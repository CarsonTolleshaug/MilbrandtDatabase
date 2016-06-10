using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using System.Threading;

namespace MilbrandtFPDB
{
    public enum DatabaseType { Flat, SingleFamily, Townhome, Carriage }


    /// <summary>
    /// A static class which handles saving and loading data to and from dataset files
    /// </summary>
    public static class DBHelper
    {
        public static string DataFile
        {
            get
            {
                return Type.ToString().ToLower() + ".xml";
            }
        }

        private static string OldDataFile
        {
            get
            {
                return Type.ToString().ToLower() + ".dat";
            }
        }

        private static DatabaseType dbType;
        public static DatabaseType Type
        {
            set
            {
                dbType = value;
            }
            get
            {
                return dbType;
            }
        }

        public static void Write(IEnumerable<SitePlan> entries)
        {
            // no need to write an empty file, plus this prevents us from accidentally clearing the list
            if (entries == null || entries.Count() == 0)
                return;

            using (XmlWriter writer = XmlWriter.Create(DataFile, new XmlWriterSettings() { Indent = true }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("EntryList");

                foreach (SitePlan s in entries)
                {
                    writer.WriteStartElement("Entry");

                    writer.WriteElementString("ID", s.ID.ToString());

                    // Write all the siteplan public properties
                    foreach (string propertyName in SitePlan.Properties)
                        writer.WriteElementString(propertyName, SitePlan.GetProperty(s, propertyName));

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }

            // rename the old data file with .bak extension, to show it's no longer in use
            if (File.Exists(OldDataFile))
            {
                File.Copy(OldDataFile, OldDataFile + ".bak", true);
                File.Delete(OldDataFile);
            }
        }

        public static List<SitePlan> Load()
        {
            List<SitePlan> entryList = new List<SitePlan>();

            if (File.Exists(DataFile))
            {
                Read((entry) =>
                {
                    SitePlan s = new SitePlan();

                    int id = ReadInt("ID", entry);
                    if (id >= 0)
                        s.ID = id;

                    // Read or get a blank value for all properties
                    foreach (string propertyName in SitePlan.Properties)
                        SitePlan.SetProperty(s, propertyName, ReadValue(propertyName, entry));

                    entryList.Add(s);
                });
            }
            else if (File.Exists(OldDataFile))
            {
                entryList = ReadOldFile(OldDataFile);
            }
            else if (File.Exists(OldDataFile + ".bak"))
            {
                entryList = ReadOldFile(OldDataFile + ".bak");
            }

            return entryList;
        }

        public static void Update(Dictionary<int, SitePlan> entries)
        {
            Read((entry) =>
            {
                int id = ReadInt("ID", entry);
                if (id >= 0)
                {
                    if (!entries.ContainsKey(id))
                    {
                        // add new entry
                        entries[id] = new SitePlan();
                    }

                    // update property values if different
                    foreach (string propertyName in SitePlan.Properties)
                        SitePlan.SetProperty(entries[id], propertyName, ReadValue(propertyName, entry));
                }
            });
        }

        private const int READ_TIMEOUT = 5000; // 5 seconds
        private static void Read(Action<XElement> entryAction)
        {
            // Try to read the file, continuously trying for 5 seconds before giving up
            string fullpath = Path.Combine(Directory.GetCurrentDirectory(), DataFile);
            bool success = TryToUseFile(fullpath,
                (sr) =>
                {
                    XDocument doc = XDocument.Load(sr, LoadOptions.PreserveWhitespace);
                    XElement list = doc.Element("EntryList");
                    if (list != null)
                    {
                        foreach (XElement entry in list.Descendants("Entry"))
                        {
                            entryAction(entry);
                        }
                    }
                },
                READ_TIMEOUT);

            // if we were unsuccesful, let the outside world handle it.
            if (!success)
                throw new IOException("Process timed out.");
        }

        private static string ReadValue(string name, XElement root)
        {
            XElement elm = root.Element(name);
            if (elm == null)
                return "";

            return elm.Value;
        }

        private static int ReadInt(string name, XElement root)
        {
            int temp;
            if (int.TryParse(ReadValue(name, root), out temp))
                return temp;
            return -1;
        }

        private static List<SitePlan> ReadOldFile(string path)
        {
            List<SitePlan> entryList = new List<SitePlan>();
            using (StreamReader sr = new StreamReader(path))
            {                
                while (!sr.EndOfStream)
                {
                    string[] data = sr.ReadLine().Split("|".ToCharArray(), 12);

                    if (data.Length >= 11)
                    {
                        SitePlan s = new SitePlan()
                        {
                            ProjectNumber = data[0],
                            ProjectName = data[1],
                            ClientName = data[2],
                            Location = data[3],
                            Plan = data[4],
                            Width = data[5],
                            Depth = data[6],
                            Beds = data[7],
                            Baths = data[8],
                            SquareFeet = data[9],
                            FilePath = data.Last()
                        };

                        if (data.Length > 11)
                            s.Date = DateTime.Parse(data[10]);

                        entryList.Add(s);
                    }
                }

                sr.Close();
            }
            return entryList;
        }


        // Obtained from CodeProject:
        // http://www.codeproject.com/Tips/164428/C-FileStream-Lock-How-to-wait-for-a-file-to-get-re
        // Modified to use StreamReader rather than FileStream
        public static bool TryToUseFile(string path, Action<StreamReader> action, int milliSecondMax = Timeout.Infinite)
        {
            bool result = false;
            DateTime dateTimestart = DateTime.Now;
            Tuple<AutoResetEvent, FileSystemWatcher> tuple = null;

            while (true)
            {
                try
                {
                    using (var file = new StreamReader(path))
                    {
                        action(file);
                        result = true;
                        break;
                    }
                }
                catch (IOException ex)
                {
                    // Init only once and only if needed. Prevent against many instantiation in case of multhreaded 
                    // file access concurrency (if file is frequently accessed by someone else). Better memory usage.
                    if (tuple == null)
                    {
                        var autoResetEvent = new AutoResetEvent(true);
                        var fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(path))
                        {
                            EnableRaisingEvents = true
                        };

                        fileSystemWatcher.Changed +=
                            (o, e) =>
                            {
                                if (Path.GetFullPath(e.FullPath) == Path.GetFullPath(path))
                                {
                                    autoResetEvent.Set();
                                }
                            };

                        tuple = new Tuple<AutoResetEvent, FileSystemWatcher>(autoResetEvent, fileSystemWatcher);
                    }

                    int milliSecond = Timeout.Infinite;
                    if (milliSecondMax != Timeout.Infinite)
                    {
                        milliSecond = (int)(DateTime.Now - dateTimestart).TotalMilliseconds;
                        if (milliSecond >= milliSecondMax)
                        {
                            result = false;
                            break;
                        }
                    }

                    tuple.Item1.WaitOne(milliSecond);
                }
            }

            if (tuple != null && tuple.Item1 != null) // Dispose of resources now (don't wait the GC).
            {
                tuple.Item1.Dispose();
                tuple.Item2.Dispose();
            }

            return result;
        }
    }    
}
