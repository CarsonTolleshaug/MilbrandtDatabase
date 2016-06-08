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
    /// <summary>
    /// A static class used to manage saving and loading settings
    /// from the global settings file.
    /// </summary>
    public static class Settings
    {
        #region Settings File Locations
        private static readonly string globalSettingsDirectory = "Global_Settings";
        private static readonly string globalSettingsFileName = "global_settings.xml";

        /// <summary>
        /// The directory containing the global settings file.
        /// Note: path may be relative
        /// </summary>
        public static string GlobalSettingsDirectory
        {
            get
            {
                return globalSettingsDirectory;
            }
        }

        /// <summary>
        /// The complete path to the global settings file including file name.
        /// Note: path may be relative
        /// </summary>
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

        // This is actually what loads all the settings initially. 
        // This is lazy loading, and will not actually load until 
        // some function or property of this class is called.
        private static bool globalInitialized = ReadGlobalSettings();

        /// <summary>
        /// A fully qualified path to the Directory to save all 
        /// generated PDF files in. This folder will contain 
        /// sub-folders for each of the datasets, which will 
        /// each also have sub-folders for project numbers.
        /// </summary>
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

        /// <summary>
        /// A fully qualified path to the jobs.dat file 
        /// created/used by the JobsList app.
        /// </summary>
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

        /// <summary>
        /// The amount to increment each value range by
        /// in the Square ft. column filter drop down.
        /// Ex: setting this to 250 will result in ranges:
        /// 0 - 250, 251 - 500, 501 - 750, ...
        /// </summary>
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

        /// <summary>
        /// A regular expression (.NET syntax) that parses
        /// out the Plan number from the filename of the
        /// floor plan .pdf file, storing the number in a 
        /// group called "digits" and any preserved suffix 
        /// (DL, DB, .2, etc) in a group called "suffix".
        /// </summary>
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

        /// <summary>
        /// Resets the PlanParseRegex to its original value
        /// stored in code as a constant.
        /// </summary>
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


        /// <summary>
        /// Gets the fully qualified path of the standard save location for generating a pdf file
        /// </summary>
        /// <param name="projectNumber">The project number of the SitePlan</param>
        /// <param name="plan">The plan number of the SitePlan</param>
        /// <returns>The path for the pdf file</returns>
        public static string GetStandardPdfFilename(string projectNumber, string plan)
        {
            // Add underscore for single family
            string typeStr = DBHelper.Type == DatabaseType.SingleFamily ? "Single_Family" : DBHelper.Type.ToString();

            return Path.Combine(PlansRootDirectory, typeStr, projectNumber, plan + ".pdf");
        }
    }
}
