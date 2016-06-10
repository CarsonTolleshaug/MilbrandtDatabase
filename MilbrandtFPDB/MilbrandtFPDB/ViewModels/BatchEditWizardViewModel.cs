using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace MilbrandtFPDB
{
    public class BatchEditWizardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<string> ErrorOccured;

        private MainWindowViewModel _mainVM;
        private IEnumerable<SitePlan> _entries;
        public const string VALUE_VARIED = "<varies>";

        public BatchEditWizardViewModel(MainWindowViewModel mainVM, IEnumerable<SitePlan> entries, 
            Dictionary<string, ObservableCollection<string>> availableValues, Dictionary<string, KeyValueWrapper> propertyValues,
            Dictionary<string, KeyValueWrapper> propertyDisplayNames)
        {
            _mainVM = mainVM;
            _entries = entries;

            foreach (SitePlan entry in _entries)
                entry.PropertyChanged += entry_PropertyChanged;

            AvailableValues = availableValues;
            PropertyValues = propertyValues;
            PropertyDisplayNames = propertyDisplayNames;

            InitializeValues();
        }

        private void entry_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnErrorOccured("Someone else has made changes to one the entries you were editing. Please try again if you wish to make additional changes.");
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
                if (property != "FilePath")
                {
                    bool first = true;
                    foreach (SitePlan sp in _entries)
                    {
                        if (first)
                        {
                            PropertyValues[property].Value = SitePlan.GetProperty(sp, property);
                            first = false;
                        }
                        else if (PropertyValues[property].Value != SitePlan.GetProperty(sp, property))
                        {
                            if (property == "Date")
                                PropertyValues[property].Value = "";
                            else
                                PropertyValues[property].Value = VALUE_VARIED;
                            break;
                        }
                    }
                }
            }

            // Property Display Names
            foreach (string property in _mainVM.ParameterDisplayNames.Keys)
            {
                PropertyDisplayNames[property].Value = _mainVM.ParameterDisplayNames[property];
            }
        }

        public void Save()
        {
            SitePlan[] spArray = _entries.ToArray();
            foreach (string property in PropertyValues.Keys)
            {
                if (PropertyValues[property].Value != VALUE_VARIED)
                {
                    for (int i = 0; i < spArray.Length; i++)
                    {
                        SitePlan sp = spArray[i];
                        SitePlan.SetProperty(sp, property, PropertyValues[property].Value);
                    }
                }
            }
        }

        public Dictionary<string, ObservableCollection<string>> AvailableValues
        {
            get;
            private set;
        }

        public Dictionary<string, KeyValueWrapper> PropertyValues
        {
            get;
            private set;
        }

        public Dictionary<string, KeyValueWrapper> PropertyDisplayNames
        {
            get;
            private set;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnErrorOccured(string errorMessage)
        {
            if (ErrorOccured != null)
                ErrorOccured(this, errorMessage);
        }
    }
}
