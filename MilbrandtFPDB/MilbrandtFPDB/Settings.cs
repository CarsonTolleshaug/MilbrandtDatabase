using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace MilbrandtFPDB
{
    public static class Settings
    {
        #region Settings File Locations
        private static readonly string localSettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FPDatabase"); //<- Defualt System Directory 
        private static readonly string localSettingsFileName = "settings.xml";
        private static readonly string globalSettingsDirectory = "Global_Settings";
        private static readonly string globalSettingsFileName = "global_settings.xml";

        public static string LocalSettingsDirectory
        {
            get
            {
                return localSettingsDirectory;
            }
        }
        public static string LocalSettingsFile
        {
            get
            {
                return Path.Combine(localSettingsDirectory, localSettingsFileName);
            }
        }

        public static string GlobalSettingsDirectory
        {
            get
            {
                return globalSettingsDirectory;
            }
        }
        public static string GlobalSettingsFile
        {
            get
            {
                return Path.Combine(globalSettingsDirectory, globalSettingsFileName);
            }
        }
        #endregion


        #region Global Settings

        private static string plansRootDirectory = @"C:\Users\carso\Documents\Milbrandt\Plans";
        private static string jobListFile = @"C:\Users\carso\Documents\Milbrandt\MilbrandtDatabase\JobsList\Milbrandt Job List\bin\x86\Release\jobs.dat";
        private static int sqftRangeStep = 250;
        private const string DEFAULT_PLAN_REGEX = @"(?(.*Plan\s*\d+)(?:.*Plan\s*)(?<digits>\d+)|(?:.*)(?<digits>\d{4}))(?:(?:.*?[\W_]+)*?(?<suffix>DL|DB|\.2))?";
        private static string planRegex = DEFAULT_PLAN_REGEX;

        private static bool globalInitialized = ReadGlobalSettings();

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

        public static string PlanParseRegex
        {
            get { return planRegex; }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    planRegex = value;
                }
            }
        }

        public static void ResetPlanRegexToDefault()
        {
            PlanParseRegex = DEFAULT_PLAN_REGEX;
        }

        public static void SaveGlobalSettings()
        {
            if (!Directory.Exists(GlobalSettingsDirectory))
                Directory.CreateDirectory(GlobalSettingsDirectory);

            using (XmlWriter writer = XmlWriter.Create(GlobalSettingsFile, new XmlWriterSettings() { Indent = true }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Settings");

                // Settings
                writer.WriteElementString("PlansRootDirectory", PlansRootDirectory);
                writer.WriteElementString("JobListFile", JobListFile);
                writer.WriteElementString("SqftRangeStep", SqftRangeStep.ToString());
                writer.WriteElementString("PlanRegex", XmlConvert.EncodeName(PlanParseRegex));

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
        }

        public static bool ReadGlobalSettings()
        {
            if (File.Exists(GlobalSettingsFile))
            {
                XDocument doc = XDocument.Load(GlobalSettingsFile, LoadOptions.PreserveWhitespace);
                XElement root = doc.Element("Settings");

                // Settings
                PlansRootDirectory = ReadValue("PlansRootDirectory", root);
                JobListFile = ReadValue("JobListFile", root);
                SqftRangeStep = ReadInt("SqftRangeStep", root);
                PlanParseRegex = XmlConvert.DecodeName(ReadValue("PlanRegex", root));
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

        #endregion

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
