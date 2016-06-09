using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

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

                    // Write all the siteplan properties
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

        public static List<SitePlan> Read()
        {
            List<SitePlan> entryList = new List<SitePlan>();

            if (File.Exists(DataFile))
            {
                XDocument doc = XDocument.Load(DataFile, LoadOptions.PreserveWhitespace);
                XElement list = doc.Element("EntryList");
                if (list != null)
                {
                    foreach(XElement entry in list.Descendants("Entry"))
                    {
                        SitePlan s = new SitePlan();

                        // Read or get a blank value for all properties
                        foreach (string propertyName in SitePlan.Properties)
                            SitePlan.SetProperty(s, propertyName, ReadValue(propertyName, entry));

                        entryList.Add(s);
                    }
                }     
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

        private static string ReadValue(string name, XElement root)
        {
            XElement elm = root.Element(name);
            if (elm == null)
                return "";

            return elm.Value;
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
    }
}
