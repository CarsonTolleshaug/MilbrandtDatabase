using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

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
                HashSet<string> distinctValues = new HashSet<string>();
                foreach(SitePlan sp in _mainVM.Entries)
                {
                    distinctValues.Add(SitePlan.GetParameter(sp, property));
                }
                List<string> sortedDistinctValues = distinctValues.ToList();
                sortedDistinctValues.Sort();
                AvailableValues[property] = new ObservableCollection<string>(sortedDistinctValues);                
            }

            // Property Values
            PropertyValues = new Dictionary<string, KeyValueWrapper>();
            foreach (string property in SitePlan.Parameters)
            {
                string value = WizardType == AddEditWizardType.Add ? "" : SitePlan.GetParameter(Entry, property);
                PropertyValues[property] = new KeyValueWrapper(property, value);
            }
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
    }
}
