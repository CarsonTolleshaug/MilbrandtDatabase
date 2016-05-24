using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace MilbrandtFPDB
{
    enum DatabaseType { Flat, SingleFamily, Townhome, Carriage }

    static class DBHelper
    {
        private static string settingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FPDatabase"); //<- Defualt System Directory 
        //"C:/Program Files/FP Database";
        public static string settingsFile
        {
            get
            {
                return Path.Combine(settingsDirectory, "layout.settings");
            }
        }
        private static string dataFile;

        private static DatabaseType dbType;
        public static DatabaseType Type
        {
            set
            {
                dbType = value;
                if (value == DatabaseType.Flat)
                    dataFile = "flat.dat";
                if (value == DatabaseType.SingleFamily)
                    dataFile = "singlefamily.dat";
                if (value == DatabaseType.Townhome)
                    dataFile = "townhome.dat";
                if (value == DatabaseType.Carriage)
                    dataFile = "carriage.dat";
            }
            get
            {
                return dbType;
            }
        }

        public static void Write(List<SitePlan> sites)
        {
            if (!File.Exists(dataFile))
            {
                using (File.Create(dataFile)) { }//closes the filestream so the method can continue.
            }

            StreamWriter sw = new StreamWriter(dataFile, false);

            foreach (SitePlan s in sites)
            {
                sw.WriteLine(s.Writestring);
            }
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

        public static List<SitePlan> Read()
        {
            List<SitePlan> retval = new List<SitePlan>();

            if (File.Exists(dataFile))
            {
                StreamReader sr = new StreamReader(dataFile);


                while (!sr.EndOfStream)
                {
                    string[] data = sr.ReadLine().Split("|".ToCharArray(), 12);
                    //if it's an old data file add the "date" parameter:
                    if (data.Length == 11)
                        retval.Add(new SitePlan(data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], "", data[10]));
                    else
                        retval.Add(new SitePlan(data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], data[10], data[11]));
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

            //if its an old settings file:
            //100 is default width of the last colomn
            if (retval.Count == 18)
                retval.Add("100");

            return retval.ToArray();
        }

        static public string WriteProjectNumber(string pn)
        {
            //removes the 19 or 20 from the project number when displaying it
            if (pn.Length >= 6)
                return pn.Remove(0, 2);
            else
                return pn;
        }

        static public string ReadProjectNumber(string pn)
        {
            if (pn.Length >= 6)
                return pn;
            //adds the 19 or 20 to the begining of the project number when storing it
            //if it begins with 80 or above use the prefix 19 (i.e. 1987)
            if (pn[0] >= '8')
                return pn.Insert(0, "19");
            else //if it begins with lower than 80 use the prefix 20 (i.e. 2012)
                return pn.Insert(0, "20");
        }

    }
}
