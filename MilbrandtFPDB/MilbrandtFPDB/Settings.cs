using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace MilbrandtFPDB
{
    public static class Settings
    {
        private static readonly string settingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FPDatabase"); //<- Defualt System Directory 
        private static readonly string settingsFileName = "settings.xml";

        public static string SettingsDirectory
        {
            get
            {
                return settingsDirectory;
            }
        }
        public static string SettingsFile
        {
            get
            {
                return Path.Combine(settingsDirectory, settingsFileName);
            }
        }
        
        private static string plansRootDirectory = @"C:\Users\carso\Documents\Milbrandt\Plans";
        private static string jobListFile = @"C:\Users\carso\Documents\Milbrandt\MilbrandtDatabase\JobsList\Milbrandt Job List\bin\x86\Release\jobs.dat";
        private static int sqftRangeStep = 250;
        private static bool initialized = ReadSettings();

        public static string PlansRootDirectory
        {
            get { return plansRootDirectory; }
            set
            {
                if (Directory.Exists(value))
                {
                    plansRootDirectory = value;
                }
            }
        }

        public static string JobListFile
        {
            get { return jobListFile; }
            set
            {
                if (File.Exists(value))
                {
                    jobListFile = value;
                }
            }
        }

        public static int SqftRangeStep
        {
            get { return sqftRangeStep; }
            set
            {
                if (value > 0)
                {
                    sqftRangeStep = value;
                }
            }
        }

        public static void SaveSettings()
        {
            if (!Directory.Exists(SettingsDirectory))
                Directory.CreateDirectory(SettingsDirectory);

            using (XmlWriter writer = XmlWriter.Create(SettingsFile, new XmlWriterSettings() { Indent = true }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Settings");

                // Settings
                writer.WriteElementString("PlansRootDirectory", PlansRootDirectory);
                writer.WriteElementString("JobListFile", JobListFile);
                writer.WriteElementString("SqftRangeStep", SqftRangeStep.ToString());

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
        }

        public static bool ReadSettings()
        {
            if (File.Exists(SettingsFile))
            {
                XDocument doc = XDocument.Load(SettingsFile, LoadOptions.PreserveWhitespace);
                XElement root = doc.Element("Settings");

                // Settings
                PlansRootDirectory = ReadValue("PlansRootDirectory", root);
                JobListFile = ReadValue("JobListFile", root);
                SqftRangeStep = ReadInt("SqftRangeStep", root);
            }

            return true;
            //List<string> retval = new List<string>();

            //if (File.Exists(settingsFile))
            //{
            //    StreamReader sr = new StreamReader(settingsFile);

            //    while (!sr.EndOfStream)
            //        retval.Add(sr.ReadLine());

            //    sr.Close();
            //}

            ////if its an old settings file:
            ////100 is default width of the last colomn
            //if (retval.Count == 18)
            //    retval.Add("100");

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
            return 0;
        }
    }
}
