using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace PlanReferenceDatabase
{
    enum DatabaseType { Flat, SingleFamily, Townhome, Carriage }
    enum SortMethod { Default, Accending, Decending }

    class SitePlan : IComparable
    {
        #region Ivar Properties
        public static SortMethod Sort
        {
            get;
            set;
        }

        public string ProjNumber
        {
            get;
            set;
        }
        public string ProjName
        {
            get;
            set;
        }
        public string ClientName
        {
            get;
            set;
        }
        public string Location
        {
            get;
            set;
        }
        public string Plan
        {
            get;
            set;
        }
        public string Width
        {
            get;
            set;
        }
        public string Depth
        {
            get;
            set;
        }
        public string Bedrooms
        {
            get;
            set;
        }
        public string Baths
        {
            get;
            set;
        }
        public string Sqrft
        {
            get;
            set;
        }
        public string Date
        {
            get;
            set;
        }
        public string LinkPath
        {
            get;
            set;
        }
        #endregion

        public SitePlan(string projNumber, string projName, string clientName, string location, string plan, string width, string depth, string beds, string baths, string sqrft, string date, string linkPath)
        {
            this.ProjNumber = projNumber;
            this.ProjName = projName;
            this.ClientName = clientName;
            this.Location = location;
            this.Plan = plan;
            this.Width = width;
            this.Depth = depth;
            this.Bedrooms = beds;
            this.Baths = baths;
            this.Sqrft = sqrft;
            this.Date = date;
            this.LinkPath = linkPath;
        }
        public SitePlan() //Constructor for creating new entries
        {
            this.ProjName = "New Plan Reference";
            this.ClientName = this.Location = this.Plan = 
                this.LinkPath = this.Date = "";
            this.ProjNumber = this.Bedrooms = this.Baths = 
                this.Sqrft = this.Width = this.Depth = "0";
        }

        /// <summary>
        /// Opens the file specifed 
        /// for the SitePlan object.
        /// </summary>
        public void Open()
        {
            Process open = new Process();
            open.StartInfo = new ProcessStartInfo(LinkPath);
            open.Start();
        }

        /// <summary>
        /// Checks whether two SitePlan
        /// objects are equal.
        /// </summary>
        /// <param name="site">The SitePlan
        /// object to compare to.</param>
        /// <returns>Returns true if the two
        /// SitePlan instances are "equal".</returns>
        public bool IsEqualTo(SitePlan site)
        {
            if (this.ProjNumber != "Any" && site.ProjNumber != this.ProjNumber)
                return false;
            if (this.ProjName != "Any" && site.ProjName != this.ProjName)
                return false;
            if (this.ClientName != "Any" && site.ClientName != this.ClientName)
                return false;
            if (this.Location != "Any" && site.Location != this.Location)
                return false;
            if (this.Plan != "Any" && site.Plan != this.Plan)
                return false;
            if (this.Width != "Any" && site.Width != this.Width)
                return false;
            if (this.Depth != "Any" && site.Depth != this.Depth)
                return false;
            if (this.Bedrooms != "Any" && site.Bedrooms != this.Bedrooms)
                return false;
            if (this.Baths != "Any" && site.Baths != this.Baths)
                return false;
            if (this.Sqrft != "Any" && this.Sqrft != "" && site.Sqrft != "")
            {
                try
                {
                    double d = double.Parse(site.Sqrft);
                    string[] bounds = this.Sqrft.Split(" - ".ToCharArray(), 2);
                    double lower = double.Parse(bounds[0]);
                    //revoves the " - " from the begining of the string
                    bounds[1] = bounds[1].Trim('-', ' ');
                    double upper = double.Parse(bounds[1]);
                    if (d >= upper || d < lower)
                        return false;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public int CompareTo(object obj) 
        {
            SitePlan sp = (SitePlan)obj;
            if (Sort == SortMethod.Default)
            {
                int pn1, pn2;
                try {pn1 = int.Parse(this.ProjNumber);} catch {return 1;}
                try {pn2 = int.Parse(sp.ProjNumber);} catch {return -1;}
                return pn1.CompareTo(pn2);
            }

            string[] dString;
            DateTime dateA;
            DateTime dateB;
            try
            {
                dString = this.Date.Split("/".ToCharArray(), 3);
                dateA = new DateTime(int.Parse(dString[2]), int.Parse(dString[0]), int.Parse(dString[1]));
            }
            catch { return 1; }
            try
            {
                dString = sp.Date.Split("/".ToCharArray(), 3);
                dateB = new DateTime(int.Parse(dString[2]), int.Parse(dString[0]), int.Parse(dString[1]));
            }
            catch { return -1; }
            int retVal = dateA.CompareTo(dateB);

            if (Sort == SortMethod.Accending)
                return retVal;
            else
                return -retVal;
        }

        /// <summary>
        /// Gets the string to write to
        /// file for the SitePlan instance.
        /// </summary>
        public string Writestring
        {
            get
            {
                return ProjNumber + "|" + ProjName + "|" + ClientName + "|" + Location 
                    + "|" + Plan + "|" + Width + "|" + Depth + "|" + Bedrooms 
                    + "|" + Baths + "|" + Sqrft + "|" + Date + "|" +  LinkPath;
            }
        }
    }

    static class DataBase
    {
        private static string settingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FPDatabase"); //<- Defualt System Directory 
            //"C:/Program Files/FP Database";
        public static string settingsFile
        {
            get
            {
                return Path.Combine(settingsDirectory ,"layout.settings");
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
