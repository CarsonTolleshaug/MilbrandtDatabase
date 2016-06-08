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
        
        public const string VALUE_ANY = "(Any)";
        private ObservableCollection<SitePlan> entries = new ObservableCollection<SitePlan>();
        private Dictionary<string, ObservableCollection<string>> _availableValues = new Dictionary<string,ObservableCollection<string>>();
        private SitePlan _selectedEntry;

        public DataGridViewModel()
        {
            DisplayedEntries = new ObservableCollection<SitePlan>();
            LoadHeaders();

            
            DBHelper.Type = DatabaseType.SingleFamily;
            LoadEntries();
        }

        private void LoadHeaders()
        {
            SelectedValues = new Dictionary<string, KeyValueWrapper>();
            ParameterDisplayNames = new Dictionary<string, string>();

            foreach (string header in SitePlan.Properties)
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

            // set non-default display names
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

        public void SaveEntries()
        {
            DBHelper.Write(Entries);
        }

        public void AddEntry(SitePlan sp, bool refreshLists = true)
        {
            entries.Add(sp);

            sp.PropertyChanged += SitePlanPropertyChanged;

            if (refreshLists)
            {
                // clear filters
                ClearFilters();

                // update all available values
                RefreshDisplay();
                UpdateAvailableValues();
            }

            if (DisplayedEntries.Contains(sp))
                SelectedEntry = sp;
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

        public void ClearFilters()
        {
            foreach (string parameter in SelectedValues.Keys)
            {
                SelectedValues[parameter].Value = VALUE_ANY;
            }
        }

        public void RefreshDisplay()
        {
            DisplayedEntries.Clear();
            foreach(SitePlan sp in entries)
            {
                bool match = true;
                foreach(string property in SitePlan.Properties)
                {
                    if (property == "SquareFeet")
                    {
                        // handle range
                        if (SelectedValues[property].Value != VALUE_ANY)
                        {
                            try
                            {
                                double d = sp.GetSquareFeetValue();
                                string[] bounds = SelectedValues[property].Value.Split(" -".ToCharArray(), 2, StringSplitOptions.RemoveEmptyEntries);
                                double lower = double.Parse(bounds[0]);
                                double upper = double.Parse(bounds[1]);
                                if (d >= upper || d < lower)
                                    match = false;
                            }
                            catch
                            {
                                match = false;
                            }
                        }
                    }
                    else if (property == "Date")
                    {
                        if (SelectedValues[property].Value != VALUE_ANY && SelectedValues[property].Value != sp.Date.Year.ToString())
                        {
                            match = false;
                        }
                    }
                    else if (property != "FilePath" && SelectedValues[property].Value != VALUE_ANY && SelectedValues[property].Value != SitePlan.GetProperty(sp, property))
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
            SitePlan temp = SelectedEntry;
            RefreshDisplay();
            UpdateAvailableValues(e.PropertyName);

            // Reselect the edited siteplan
            SelectedEntry = temp;
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

            List<string> orderedDistinctValues;

            // handle sq ft differently, because it has range values
            if (propertyName == "SquareFeet")
            {
                orderedDistinctValues = new List<string>();
                orderedDistinctValues.Add(VALUE_ANY);

                double max = Entries.Max(m => m.GetSquareFeetValue());
                int step = Settings.SqftRangeStep;
                for (int i = 0; i < max; i += step)
                {
                    orderedDistinctValues.Add(string.Format("{0} - {1}", i == 0 ? 0 : i + 1, i + step));
                }
            }
            else
            {
                // hashset to eliminate duplicate values
                HashSet<string> distinctValues = new HashSet<string>();

                foreach(SitePlan entry in DisplayedEntries)
                {
                    string val;
                    if (propertyName == "Date")
                        val = entry.Date.Year.ToString();
                    else
                        val = SitePlan.GetProperty(entry, propertyName);
                    distinctValues.Add(val);
                }

                orderedDistinctValues = distinctValues.ToList();
                orderedDistinctValues.Sort();
                orderedDistinctValues.Insert(0, VALUE_ANY);
            }

            string selectedValue = SelectedValues[propertyName].Value;
           
            if (_availableValues[propertyName].Count == 0)
            {
                foreach(string s in orderedDistinctValues)
                {
                    _availableValues[propertyName].Add(s);
                }
            }
            else
            {
                // Make the new availableValues[propertyName] list match the orderedDistinctValues one
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

        public SitePlan SelectedEntry
        {
            get { return _selectedEntry; }
            set
            {
                if (_selectedEntry != value)
                {
                    _selectedEntry = value;
                    OnPropertyChanged("SelectedEntry");
                }
            }
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
                    // save changes before we switch
                    DBHelper.Write(Entries);

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
