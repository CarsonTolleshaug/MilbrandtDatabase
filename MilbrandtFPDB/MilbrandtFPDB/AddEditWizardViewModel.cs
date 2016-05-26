using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;

namespace MilbrandtFPDB
{
    public enum AddEditWizardType { Add, Edit }

    public class AddEditWizardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private AddEditWizardType _type;
        private SitePlan _entry;
        private DataGridViewModel _mainVM;
        private string _filepath;

        public AddEditWizardViewModel(AddEditWizardType type, DataGridViewModel mainVM, SitePlan entry)
        {
            _type = type;
            _mainVM = mainVM;
            _entry = type == AddEditWizardType.Edit ? entry : new SitePlan();
            _filepath = "";

            InitializeValues();
        }

        private void InitializeValues()
        {
            // Available Values
            AvailableValues = new Dictionary<string, ObservableCollection<string>>();
            foreach (string property in SitePlan.Parameters)
            {
                if (property != "FilePath")
                {
                    HashSet<string> distinctValues = new HashSet<string>();
                    foreach (SitePlan sp in _mainVM.Entries)
                    {
                        distinctValues.Add(SitePlan.GetParameter(sp, property));
                    }
                    List<string> sortedDistinctValues = distinctValues.ToList();
                    sortedDistinctValues.Sort();
                    AvailableValues[property] = new ObservableCollection<string>(sortedDistinctValues);
                }
            }

            // Property Values
            PropertyValues = new Dictionary<string, KeyValueWrapper>();
            foreach (string property in SitePlan.Parameters)
            {                
                string value = WizardType == AddEditWizardType.Add ? "" : SitePlan.GetParameter(Entry, property);

                if (property == "FilePath")
                {
                    FilePath = value;
                }
                else
                {
                    PropertyValues[property] = new KeyValueWrapper(property, value);
                    if (property == "ProjectNumber")
                        PropertyValues[property].PropertyChanged += ProjectNumberChanged;
                }
            }
        }

        private void ProjectNumberChanged(object sender, PropertyChangedEventArgs e)
        {
            AutofillFromProjectNumber();
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

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string GetParameterDisplayName(string parameterName)
        {
            return _mainVM.ParameterDisplayNames[parameterName];
        }

        public void Save()
        {
            // This should never happen, but just a precaution
            if (Entry == null)
                _entry = new SitePlan();

            // Force FilePath to be filled in
            if (String.IsNullOrWhiteSpace(FilePath))
            {
                throw new ArgumentException(GetParameterDisplayName("FilePath") + " cannot be blank.");
            }

            // Set the properties of the site plan object
            Entry.FilePath = FilePath;
            foreach (string property in PropertyValues.Keys)
            {
                SitePlan.SetParameter(Entry, property, PropertyValues[property].Value);
            }

            if (_type == AddEditWizardType.Add)
            {
                _mainVM.AddEntry(Entry);
            }
            
            //Todo: tell _mainVM to save to file
        }

        public void AutofillFromFilePath()
        {
            if (String.IsNullOrWhiteSpace(FilePath))
                return;

            // Plan from filename
            if (String.IsNullOrWhiteSpace(PropertyValues["Plan"].Value))
            {
                string plan = Path.GetFileNameWithoutExtension(FilePath);
                PropertyValues["Plan"].Value = plan.Replace('_', ' ');
            }

            // Project Number and Plan from path
            if (String.IsNullOrWhiteSpace(PropertyValues["ProjectNumber"].Value))
            {
                string projNum = Directory.GetParent(FilePath).Name;
                PropertyValues["ProjectNumber"].Value = projNum;
            }
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
                PropertyValues["ProjectName"].Value = sitePlanWithSamePN.ProjectName;
                PropertyValues["ClientName"].Value = sitePlanWithSamePN.ClientName;
                PropertyValues["Location"].Value = sitePlanWithSamePN.Location;
                PropertyValues["Date"].Value = sitePlanWithSamePN.Date;
            }
        }
    }
}
