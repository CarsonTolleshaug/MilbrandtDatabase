using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;

namespace MilbrandtFPDB
{
    enum SortMethod { Default, Accending, Decending }

    public class SitePlan : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _projNum, _projName, _clientName, _location, _type, _plan, _width, _depth, _beds, _baths, _sqft, _filePath;
        private DateTime _date;


        #region Properties

        internal static SortMethod Sort
        {
            get;
            set;
        }

        // NOTE: Add new params here

        public string ProjectNumber
        {
            get { return _projNum; }
            set 
            { 
                if (_projNum != value)
                {
                    _projNum = value;
                    OnProperyChanged("ProjectNumber");
                }
            }
        }
        public string ProjectName
        {
            get { return _projName; }
            set 
            { 
                if (_projName != value)
                {
                    _projName = value;
                    OnProperyChanged("ProjectName");
                }
            }
        }
        public string ClientName
        {
            get { return _clientName; }
            set 
            { 
                if (_clientName != value)
                {
                    _clientName = value;
                    OnProperyChanged("ClientName");
                }
            }
        }
        public string Location
        {
            get { return _location; }
            set 
            { 
                if (_location != value)
                {
                    _location = value;
                    OnProperyChanged("Location");
                }
            }
        }
        public string Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnProperyChanged("Type");
                }
            }
        }
        public string Plan
        {
            get { return _plan; }
            set 
            { 
                if (_plan != value)
                {
                    _plan = value;
                    OnProperyChanged("Plan");
                }
            }
        }
        public string Width
        {
            get { return _width; }
            set 
            { 
                if (_width != value)
                {
                    _width = value;
                    OnProperyChanged("Width");
                }
            }
        }
        public string Depth
        {
            get { return _depth; }
            set 
            { 
                if (_depth != value)
                {
                    _depth = value;
                    OnProperyChanged("Depth");
                }
            }
        }
        public string Beds
        {
            get { return _beds; }
            set 
            { 
                if (_beds != value)
                {
                    _beds = value;
                    OnProperyChanged("Beds");
                }
            }
        }
        public string Baths
        {
            get { return _baths; }
            set 
            { 
                if (_baths != value)
                {
                    _baths = value;
                    OnProperyChanged("Baths");
                }
            }
        }
        public string SquareFeet
        {
            get { return _sqft; }
            set 
            { 
                if (_sqft != value)
                {
                    _sqft = value;
                    OnProperyChanged("SquareFeet");
                }
            }
        }
        public DateTime Date
        {
            get { return _date; }
            set 
            { 
                if (_date != value)
                {
                    _date = value;
                    OnProperyChanged("Date");
                }
            }
        }
        public string FilePath
        {
            get { return _filePath; }
            set 
            { 
                if (_filePath != value)
                {
                    _filePath = value;
                    OnProperyChanged("FilePath");
                }
            }
        }

        #endregion

        private static List<string> _properties;
        internal static List<string> Properties
        {
            get
            {
                if (_properties == null)
                {
                    _properties = new List<string>();
                    Type type = typeof(SitePlan);
                    foreach (PropertyInfo pi in type.GetProperties())
                        _properties.Add(pi.Name);
                }

                return _properties;
            }
        }
        public static string GetProperty(SitePlan sitePlan, string parameterName)
        {
            if (parameterName == "Date")
                return sitePlan.Date.ToShortDateString();

            Type type = typeof(SitePlan);
            PropertyInfo pi = type.GetProperty(parameterName);
            return pi.GetValue(sitePlan).ToString();
        }
        public static void SetProperty(SitePlan sitePlan, string parameterName, string value)
        {
            if (parameterName == "Date")
            {
                DateTime dt;
                if (DateTime.TryParse(value, out dt))
                    sitePlan.Date = dt;
            }
            else
            {
                Type type = typeof(SitePlan);
                PropertyInfo pi = type.GetProperty(parameterName);
                pi.SetValue(sitePlan, value);
            }
        }

        public SitePlan() //Constructor for creating new entries
        {
            // Add new params default values here
            this.ProjectName = "New Plan Reference";
            this.ClientName = this.Location = this.Type = this.Plan =
                this.FilePath = "";
            this.Date = DateTime.MinValue;
            this.ProjectNumber = this.Beds = this.Baths =
                this.SquareFeet = this.Width = this.Depth = "0";
        }

        /// <summary>
        /// Opens the file specifed 
        /// for the SitePlan object.
        /// </summary>
        public void Open()
        {
            Process open = new Process();
            open.StartInfo = new ProcessStartInfo(FilePath);
            open.Start();
        }

        public double GetSquareFeetValue()
        {
            double temp;
            if (double.TryParse(SquareFeet, out temp))
                return temp;
            return 0;
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
            // NOTE: Add new params here
            if (this.ProjectNumber != "Any" && site.ProjectNumber != this.ProjectNumber)
                return false;
            if (this.ProjectName != "Any" && site.ProjectName != this.ProjectName)
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
            if (this.Beds != "Any" && site.Beds != this.Beds)
                return false;
            if (this.Baths != "Any" && site.Baths != this.Baths)
                return false;
            if (this.SquareFeet != "Any" && this.SquareFeet != "" && site.SquareFeet != "")
            {
                try
                {
                    double d = double.Parse(site.SquareFeet);
                    string[] bounds = this.SquareFeet.Split(" - ".ToCharArray(), 2);
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
                try { pn1 = int.Parse(this.ProjectNumber); }
                catch { return 1; }
                try { pn2 = int.Parse(sp.ProjectNumber); }
                catch { return -1; }
                return pn1.CompareTo(pn2);
            }

            string[] dString;
            DateTime dateA;
            DateTime dateB;
            try
            {
                dString = this.Date.ToShortDateString().Split("/".ToCharArray(), 3);
                dateA = new DateTime(int.Parse(dString[2]), int.Parse(dString[0]), int.Parse(dString[1]));
            }
            catch { return 1; }
            try
            {
                dString = sp.Date.ToShortDateString().Split("/".ToCharArray(), 3);
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
        internal string Writestring
        {
            get
            {
                // NOTE: Add new params here
                return ProjectNumber + "|" + ProjectName + "|" + ClientName + "|" + Location
                    + "|" + Plan + "|" + Width + "|" + Depth + "|" + Beds
                    + "|" + Baths + "|" + SquareFeet + "|" + Date + "|" + FilePath;
            }
        }

        private void OnProperyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
