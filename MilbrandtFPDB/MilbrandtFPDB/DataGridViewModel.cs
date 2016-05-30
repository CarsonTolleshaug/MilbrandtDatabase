using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;


namespace MilbrandtFPDB
{
    public class DataGridViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private const string VALUE_ANY = "(Any)";
        private ObservableCollection<SitePlan> entries = new ObservableCollection<SitePlan>();
        private Dictionary<string, ObservableCollection<string>> _availableValues = new Dictionary<string,ObservableCollection<string>>();

        public DataGridViewModel()
        {
            DisplayedEntries = new ObservableCollection<SitePlan>();
            LoadHeaders();

            // setting this automatically calls LoadEntries()
            SelectedDatabase = DatabaseType.SingleFamily;
        }

        private void LoadHeaders()
        {
            SelectedValues = new Dictionary<string, KeyValueWrapper>();
            ParameterDisplayNames = new Dictionary<string, string>();

            foreach (string header in SitePlan.Parameters)
            {
                if (header != "FilePath")
                {
                    _availableValues.Add(header, new ObservableCollection<string>());
                    SelectedValues[header] = new KeyValueWrapper(header, VALUE_ANY);
                    SelectedValues[header].PropertyChanged += SelectedFilterValueChanged;
                }

                // default display name is just the property name
                ParameterDisplayNames[header] = header;
            }

            ParameterDisplayNames["ProjectName"] = "Project Name";
            ParameterDisplayNames["ProjectNumber"] = "Project #";
            ParameterDisplayNames["ClientName"] = "Client Name";
            ParameterDisplayNames["SquareFeet"] = "Square Ft.";
            ParameterDisplayNames["FilePath"] = "PDF File Location";
        }

        private void SelectedFilterValueChanged(object sender, PropertyChangedEventArgs e)
        {
            RefreshDisplay();
            UpdateAvailableValues();
        }

        private void LoadEntries()
        {
            entries.Clear();
            foreach (SitePlan sp in DBHelper.Read())
            {
                AddEntry(sp, false);
            }

            ResetHeaderComboBoxes();
            RefreshDisplay();
            UpdateAvailableValues();
        }

        public void AddEntry(SitePlan sp, bool refreshLists = true)
        {
            entries.Add(sp);

            sp.PropertyChanged += SitePlanPropertyChanged;

            if (refreshLists)
            {
                // update all available values
                RefreshDisplay();
                UpdateAvailableValues();
            }
        }

        public void RemoveEntry(SitePlan sp)
        {
            if (entries.Remove(sp))
            {
                DisplayedEntries.Remove(sp);

                // update all available values
                //UpdateAvailableValues();
                //RefreshDisplay();
            }
        }

        public void RefreshDisplay()
        {
            DisplayedEntries.Clear();
            foreach(SitePlan sp in entries)
            {
                bool match = true;
                foreach(string property in SitePlan.Parameters)
                {
                    if (property != "FilePath" && SelectedValues[property].Value != VALUE_ANY && SelectedValues[property].Value != SitePlan.GetParameter(sp, property))
                    {
                        match = false;
                    }
                }
                if (match)
                {
                    DisplayedEntries.Add(sp);
                }
            }
        }

        private void ResetHeaderComboBoxes()
        {
            foreach (string key in SelectedValues.Keys)
            {
                SelectedValues[key].Value = VALUE_ANY;
            }
        }

        private void SitePlanPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RefreshDisplay();
            UpdateAvailableValues(e.PropertyName);
        }

        public void UpdateAvailableValues(string propertyName = null)
        {
            // update all values if property name is null
            if (propertyName == null)
            {
                foreach (string pName in _availableValues.Keys)
                    UpdateAvailableValues(pName);
                return;
            }
            else if (!_availableValues.Keys.Contains(propertyName))
            {
                return;
            }

            // hashset to eliminate duplicate values
            HashSet<string> distinctValues = new HashSet<string>();

            // Reflection type to get property from string
            Type type = typeof(SitePlan);
            PropertyInfo propInfo = type.GetProperty(propertyName);

            foreach(SitePlan entry in DisplayedEntries)
            {
                string val = propInfo.GetValue(entry).ToString();
                distinctValues.Add(val);
            }

            List<string> orderedDistinctValues = distinctValues.ToList();
            orderedDistinctValues.Sort();
            orderedDistinctValues.Insert(0, VALUE_ANY);
            string selectedValue = SelectedValues[propertyName].Value;


            // Make the new availableValues[propertyName] list match the orderedDistinctValues one
            if (_availableValues[propertyName].Count == 0)
            {
                foreach(string s in orderedDistinctValues)
                {
                    _availableValues[propertyName].Add(s);
                }
            }
            else
            {
                int i = 0, j = 0;
                while (i < _availableValues[propertyName].Count || j < orderedDistinctValues.Count)
                {
                    if (j >= orderedDistinctValues.Count)
                    {
                        // remove one
                        _availableValues[propertyName].RemoveAt(i);
                    }
                    else if (i >= _availableValues[propertyName].Count)
                    {
                        //add one
                        _availableValues[propertyName].Add(orderedDistinctValues[j]);
                    }
                    else if (_availableValues[propertyName][i].CompareTo(orderedDistinctValues[j]) < 0)
                    {
                        // remove one
                        _availableValues[propertyName].RemoveAt(i);
                    }
                    else if (_availableValues[propertyName][i].CompareTo(orderedDistinctValues[j]) > 0)
                    {
                        //add one
                        _availableValues[propertyName].Insert(j, orderedDistinctValues[j]);
                    }
                    else
                    {
                        // They are the same, so move to next item in each list
                        i++;
                        j++;
                    }
                }
            }

            if (!orderedDistinctValues.Contains(selectedValue))
                SelectedValues[propertyName].Value = VALUE_ANY;
        }

        public ObservableCollection<SitePlan> Entries
        {
            get { return entries; }
        }

        public ObservableCollection<SitePlan> DisplayedEntries
        {
            get;
            private set;
        }

        public Collection<DatabaseType> DatabaseTypes
        {
            get
            {
                Collection<DatabaseType> types = new Collection<DatabaseType>();
                
                foreach (DatabaseType t in Enum.GetValues(typeof(DatabaseType)))
                {
                    types.Add(t);
                }

                return types;
            }
        }

        public DatabaseType SelectedDatabase
        {
            get { return DBHelper.Type; }
            set
            {
                if (DBHelper.Type != value)
                {
                    DBHelper.Type = value;
                    OnPropertyChanged("SelectedDatabase");
                    LoadEntries();
                }
            }
        }

        public Dictionary<string, ObservableCollection<string>> AvailableValues
        {
            get { return _availableValues; }
        }

        public Dictionary<string, KeyValueWrapper> SelectedValues
        {
            get;
            private set;
        }

        public Dictionary<string, string> ParameterDisplayNames
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
    }


    // This class is need to make a dictionary-esc collection that can be two-way binded to
    public class KeyValueWrapper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _key, _value;

        public KeyValueWrapper(string key, string value)
        {
            _key = key;
            _value = value;
        }

        public string Key
        {
            get
            {
                return _key;
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
