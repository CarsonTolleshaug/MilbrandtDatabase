using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace MilbrandtFPDB
{
    public enum AddEditWizardType { Add, Edit }

    public class AddEditWizardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<string> ErrorOccured;

        private AddEditWizardType _type;
        private SitePlan _entry;
        private MainWindowViewModel _mainVM;
        private string _filepath;
        private string _tempPath;
        private bool _raiseErrorOnPropertyChanged;

        // for add mode only
        private string _floorPlanPath;

        public AddEditWizardViewModel(AddEditWizardType type, MainWindowViewModel mainVM, SitePlan entry, 
            Dictionary<string, ObservableCollection<string>> availableValues, Dictionary<string, KeyValueWrapper> propertyValues,
            Dictionary<string, KeyValueWrapper> propertyDisplayNames)
        {
            _type = type;
            _mainVM = mainVM;
            _entry = type == AddEditWizardType.Edit ? entry : new SitePlan();
            _filepath = _floorPlanPath = "";

            if (type == AddEditWizardType.Edit)
                _entry.PropertyChanged += EntryPropertyChanged;
            _raiseErrorOnPropertyChanged = true;

            AvailableValues = availableValues;
            PropertyValues = propertyValues;
            PropertyDisplayNames = propertyDisplayNames;

            InitializeValues();
        }

        private void EntryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_raiseErrorOnPropertyChanged)
            {
                _raiseErrorOnPropertyChanged = false;
                OnErrorOccured("Someone else has made changes to the entry you were editing. Please try again if you wish to make additional changes.");
            }
        }

        private void InitializeValues()
        {
            // Available Values
            foreach (string property in SitePlan.Properties)
            {
                // FilePath and Date do not need available values
                if (property != "FilePath" && property != "Date")
                {
                    HashSet<string> distinctValues = new HashSet<string>();
                    foreach (SitePlan sp in _mainVM.Entries)
                    {
                        distinctValues.Add(SitePlan.GetProperty(sp, property));
                    }
                    List<string> sortedDistinctValues = distinctValues.ToList();
                    sortedDistinctValues.Sort();
                    AvailableValues[property].Clear();
                    foreach (string value in sortedDistinctValues)
                        AvailableValues[property].Add(value);
                }
            }

            // Property Values
            foreach (string property in SitePlan.Properties)
            {                
                string value = WizardType == AddEditWizardType.Add ? "" : SitePlan.GetProperty(Entry, property);

                if (property == "FilePath")
                {
                    FilePath = value;
                }
                else
                {
                    PropertyValues[property].Value = value;

                    if (property == "ProjectNumber")
                        PropertyValues[property].PropertyChanged += ProjectNumberChanged;
                }
            }

            // Property Display Names
            foreach (string property in _mainVM.ParameterDisplayNames.Keys)
            {
                PropertyDisplayNames[property].Value = _mainVM.ParameterDisplayNames[property];
            }

            // Aditional PDF Paths
            AdditionalPdfPaths = new ObservableCollection<KeyValueWrapper>();
        }

        public AddEditWizardType WizardType
        {
            get { return _type; }
            private set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged("WizardType");
                    OnPropertyChanged("TitleText");
                }
            }
        }

        public string TitleText
        {
            get
            {
                if (WizardType == AddEditWizardType.Add)
                    return "Add New Entry";
                else
                    return "Edit Entry";
            }
        }

        public string FilePath
        {
            get
            {
                return _filepath;
            }
            set
            {
                if (_filepath != value)
                {
                    if (String.IsNullOrWhiteSpace(value))
                        _filepath = "";
                    else
                        _filepath = value;

                    OnPropertyChanged("FilePath");
                }
            }
        }

        public string FloorPlanPath
        {
            get { return _floorPlanPath; }
            set
            {
                if (_floorPlanPath != value)
                {
                    if (String.IsNullOrWhiteSpace(value))
                        _floorPlanPath = "";
                    else
                        _floorPlanPath = value;

                    OnPropertyChanged("FloorPlanPath");
                }
            }
        }

        public ObservableCollection<KeyValueWrapper> AdditionalPdfPaths
        {
            get;
            private set;
        }

        public SitePlan Entry
        {
            get { return _entry; }
        }

        public Dictionary<string, KeyValueWrapper> PropertyValues
        {
            get;
            private set;
        }

        public Dictionary<string, ObservableCollection<string>> AvailableValues
        {
            get;
            private set;
        }

        public Dictionary<string, KeyValueWrapper> PropertyDisplayNames
        {
            get;
            private set;
        }

        public System.Windows.Visibility AddControlVisibility
        {
            get { return WizardType == AddEditWizardType.Add ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; }
        }

        public System.Windows.Visibility EditControlVisibility
        {
            get { return WizardType == AddEditWizardType.Edit ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnErrorOccured(string message)
        {
            if (ErrorOccured != null)
            {
                ErrorOccured(this, message);
            }
        }

        public void Save()
        {
            // This should never happen, but just a precaution
            if (Entry == null)
                _entry = new SitePlan();

            // This may throw exceptions which are intentionally uncaught here
            if (WizardType == AddEditWizardType.Add)
                BuildCompositePdf(FloorPlanPath, true);
            else
                BuildCompositePdf(FilePath, false);

            // Force FilePath to be filled in
            if (String.IsNullOrWhiteSpace(FilePath))
                throw new ArgumentException(PropertyDisplayNames["FilePath"] + " cannot be blank.");

            // Remove event handler so we don't set it off.
            _raiseErrorOnPropertyChanged = false;

            // Set the properties of the site plan object
            Entry.FilePath = FilePath;
            foreach (string property in PropertyValues.Keys)
            {
                SitePlan.SetProperty(Entry, property, PropertyValues[property].Value);
            }

            if (_type == AddEditWizardType.Add)
            {
                _mainVM.AddEntry(Entry);
            }
        }

        private void BuildCompositePdf(string mainPdfPath, bool createNewFile)
        {
            #region Value Checking
            // Force FloorPlanPath to be filled in
            if (String.IsNullOrWhiteSpace(mainPdfPath))
                throw new ArgumentException("Floor Plan cannot be blank");
                
            // Force FloorPlanPath to exist
            if (!File.Exists(mainPdfPath))
                throw new ArgumentException("Unable to find floor plan file");

            // Force pdf paths to exist if specified
            foreach (KeyValueWrapper pdfPath in AdditionalPdfPaths)
            {
                if (!String.IsNullOrWhiteSpace(pdfPath.Value))
                {
                    if (!File.Exists(pdfPath.Value))
                        throw new ArgumentException("Unable to find Pdf:\n" + pdfPath.Value);
                }
            }

            string projNum = PropertyValues["ProjectNumber"].Value;
            string plan = PropertyValues["Plan"].Value;

            // Force ProjectNumber to be specified
            if (String.IsNullOrWhiteSpace(projNum))
                throw new ArgumentException(_mainVM.ParameterDisplayNames["ProjectNumber"] + " cannot be blank");

            // Force Plan to be specified
            if (String.IsNullOrWhiteSpace(plan))
                throw new ArgumentException(_mainVM.ParameterDisplayNames["Plan"] + " cannot be blank");
            #endregion

            if (createNewFile)
            {
                // Set our file path to export to
                FilePath = Settings.GetStandardPdfFilename(projNum, plan);

                // Create the parent folder if it does not exist
                string parentDir = Directory.GetParent(FilePath).FullName;
                if (!Directory.Exists(parentDir))
                    Directory.CreateDirectory(parentDir);

                // Make sure FilePath does not already exist
                for (int i = 0; File.Exists(FilePath); i++)
                    FilePath = Settings.GetStandardPdfFilename(projNum, plan + "_" + i);
            }

            // use the temp pdf builder and then copy it to the file path location
            string temp_path = BuildTempCompositePdf();
            if (temp_path == null)
            {
                throw new ArgumentException("Unable to find floor plan file");
            }

            File.Copy(temp_path, FilePath, true);
        }

        public string BuildTempCompositePdf()
        {
            string mainPdf = WizardType == AddEditWizardType.Add ? FloorPlanPath : FilePath;
            if (!File.Exists(mainPdf))
                return null;

            if (!File.Exists(_tempPath))
            {
                // make a new temp file
                _tempPath = Path.Combine(Path.GetTempPath(), "mb_fpdb.pdf");
            }

            // Create output doc
            PdfSharp.Pdf.PdfDocument outputDoc = new PdfSharp.Pdf.PdfDocument();

            // Add floor plan pages
            AddPagesFromFileToDoc(mainPdf, outputDoc);

            // Add additional pdfs
            foreach (KeyValueWrapper pdfPath in AdditionalPdfPaths)
            {
                if (File.Exists(pdfPath.Value))
                    AddPagesFromFileToDoc(pdfPath.Value, outputDoc);
            }

            // Save the new combined document in the previously generated temp location
            using (FileStream file = File.Open(_tempPath, FileMode.Create, FileAccess.Write))
            {
                outputDoc.Save(file);
            }

            return _tempPath;
        }

        private void AddPagesFromFileToDoc(string inputFilename, PdfSharp.Pdf.PdfDocument outputDoc)
        {
            using (FileStream fs = File.Open(inputFilename, FileMode.Open))
            {
                PdfSharp.Pdf.PdfDocument inputDoc = PdfSharp.Pdf.IO.PdfReader.Open(fs, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
                foreach (PdfSharp.Pdf.PdfPage page in inputDoc.Pages)
                    outputDoc.AddPage(page);
                inputDoc.Close();
            }
        }

        public void AutofillFromFloorPlan()
        {
            if (String.IsNullOrWhiteSpace(FloorPlanPath))
                return;

            // ************************* Plan from filename ************************** //
            string filename = Path.GetFileNameWithoutExtension(FloorPlanPath);
            // If the filename contains the word "plan", collect digits after the plan (and after any whitespace after "plan").
            // If it does not contain "plan", collect any section of 4 digits.
            // Lastly, after the digits find the first instance of "DL","DB", or ".2" which may be preceded by any 
            // non-alphanumeric character (including underscore) and may have alphanumeric characters in between it and the 
            // digits so long as there is a non-alphanumeric separater before the suffix 
            // [Ex: "A10 Plan 4F (DL)" will match "4" as the digits and "DL" as the suffix]
            string digits = "", suffix = "";
            try
            {
                Match m = Regex.Match(filename, Settings.PlanParseRegex, RegexOptions.IgnoreCase);
                digits = m.Groups["digits"].Value;
                suffix = m.Groups["suffix"].Value;
            }
            catch { }
            if (digits.Length > 0)
            {
                StringBuilder plan = new StringBuilder();
                
                if (digits.Length < 4)
                    plan.Append("Plan ");
                plan.Append(digits);
                if (suffix.Length > 0)
                {
                    // always append DL (instead of DB or .2)
                    plan.Append(" DL");
                }

                SetPropertyIfEmpty("Plan", plan.ToString());
            }
            // *********************************************************************** //


            // ************************** Width from filename ************************ //
            if (digits.Length == 4)
            {
                // width is the first two values
                string width = string.Format("{0}\'-0\"", digits.Substring(0, 2));
                SetPropertyIfEmpty("Width", width);
            }
            // *********************************************************************** //


            // ********************* Date from file modified time ******************** //
            try
            {
                DateTime lastModified = File.GetLastWriteTime(FloorPlanPath);
                SetPropertyIfEmpty("Date", lastModified.ToShortDateString());
            }
            catch { }
            // *********************************************************************** //


            // ********************** Project Number from path *********************** //
            try
            {
                // get first 4 chars of the first directory after the drive
                string projNum = FloorPlanPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)[1];
                SetPropertyIfEmpty("ProjectNumber", projNum.Substring(0, 4));
            }
            catch { }
            // *********************************************************************** //
        }

        private void ProjectNumberChanged(object sender, PropertyChangedEventArgs e)
        {
            AutofillFromProjectNumber();
        }

        public void AutofillFromProjectNumber()
        {
            string projNum = PropertyValues["ProjectNumber"].Value;
            if (String.IsNullOrWhiteSpace(projNum))
                return;

            // See if there's another entry with the same proj #
            SitePlan sitePlanWithSamePN = _mainVM.Entries.FirstOrDefault(m => m.ProjectNumber == projNum);
            if (sitePlanWithSamePN != null)
            {
                SetPropertyIfEmpty("ProjectName", sitePlanWithSamePN.ProjectName);
                SetPropertyIfEmpty("ClientName", sitePlanWithSamePN.ClientName);
                SetPropertyIfEmpty("Location", sitePlanWithSamePN.Location);
                SetPropertyIfEmpty("Date", sitePlanWithSamePN.Date.ToShortDateString());
            }
            else
            {
                // Read from the Jobs list to try to autofill info
                Dictionary<string, string> jobListInfo = JobListReader.GetJobInfo(projNum);
                if (jobListInfo != null)
                {
                    SetPropertyIfEmpty("ProjectName", jobListInfo["ProjectName"]);
                    SetPropertyIfEmpty("ClientName", jobListInfo["ClientName"]);
                }
            }
        }

        private void SetPropertyIfEmpty(string propertyName, string value)
        {
            if (String.IsNullOrWhiteSpace(PropertyValues[propertyName].Value))
                PropertyValues[propertyName].Value = value;
        }
    }
}
