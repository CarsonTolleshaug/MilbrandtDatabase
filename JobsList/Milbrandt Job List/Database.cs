using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace HyperlinkJobsList
{
    static class Database
    {
        private static string settingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Hyperlink Jobs");
        private static string dataFile = "jobs.dat";
        public static string empFile = "employees.dat";
        
        private static string settingsFile
        {
            get
            {
                return Path.Combine(settingsDirectory, "layout.settings");
            }
        }

        public static void Write(List<Job> jobs)
        {
            if (!File.Exists(dataFile))
            {
                using (File.Create(dataFile)) { }//closes the filestream so the method can continue.
            }

            StreamWriter sw = new StreamWriter(dataFile, false);

            foreach (Job j in jobs)
            {
                sw.WriteLine(j.Writestring);
            }
            sw.Close();
        }

        public static bool Replace(Job old_j, Job new_j)
        {
            StreamReader streamReader = new StreamReader(dataFile);
            StreamWriter streamWriter = new StreamWriter(dataFile + ".tmp");
            bool successful = false;

            while (!streamReader.EndOfStream)
            {
                string data = streamReader.ReadLine();
                if (old_j.Writestring.Equals(data))
                {
                    successful = true;
                    data = new_j.Writestring;
                }
                streamWriter.WriteLine(data);
            }

            streamReader.Close();
            streamWriter.Close();

            FileInfo newFile = new FileInfo(dataFile + ".tmp");
            FileInfo oldFile = new FileInfo(dataFile);
            oldFile.Delete();
            newFile.MoveTo(dataFile);
            return successful;
        }


        public static void Append(Job j)
        {
            if (!File.Exists(dataFile))
            {
                using (File.Create(dataFile)) { }//closes the filestream so the method can continue.
            }

            StreamWriter sw = new StreamWriter(dataFile, true);
            sw.WriteLine(j.Writestring);
            sw.Close();
        }

        public static void WriteLayout(string[] settings)
        {
            if (!Directory.Exists(settingsDirectory))
                Directory.CreateDirectory(settingsDirectory);

            StreamWriter sw = new StreamWriter(settingsFile, false);

            foreach (string s in settings)
                sw.WriteLine(s);

            sw.Close();
        }

        public static List<Job> Read()
        {
            List<Job> retval = new List<Job>();

            if (File.Exists(dataFile))
            {
                StreamReader sr = new StreamReader(dataFile);

                while (!sr.EndOfStream)
                {
                    string[] data = sr.ReadLine().Split("|".ToCharArray());
                    if (data.Length != 7) // Old file
                    {
                        sr.Close();
                        ConvertOldFile();
                        sr = new StreamReader(dataFile);
                        data = sr.ReadLine().Split("|".ToCharArray());
                    }
                    retval.Add(new Job(data[0], data[1], data[2], data[3], data[4], data[5], data[6]));
                }

                sr.Close();
            }

            return retval;
        }

        public static string[] ReadLayout()
        {
            List<string> retval = new List<string>();

            if (File.Exists(settingsFile))
            {
                StreamReader sr = new StreamReader(settingsFile);

                while (!sr.EndOfStream)
                    retval.Add(sr.ReadLine());

                sr.Close();
            }

            return retval.ToArray();
        }

        public static void ConvertOldFile()
        {
            MessageBox.Show("It appears you have an old data file.\nPlease wait while it is converted.");

            StreamReader streamReader = new StreamReader(dataFile);
            StreamWriter streamWriter = new StreamWriter(dataFile + ".tmp");

            while (!streamReader.EndOfStream)
            {
                string data = streamReader.ReadLine();
                string[] tokens = data.Split("|".ToCharArray());
                if (tokens.Length == 4)
                    data += "|||";
                else if (tokens.Length == 5)
                    data += "||";
                else if (tokens.Length == 6)
                    data += "|";
                streamWriter.WriteLine(data);
            }

            streamReader.Close();
            streamWriter.Close();

            FileInfo newFile = new FileInfo(dataFile + ".tmp");
            FileInfo oldFile = new FileInfo(dataFile);
            oldFile.Delete();
            newFile.MoveTo(dataFile);
        }
    }
}
