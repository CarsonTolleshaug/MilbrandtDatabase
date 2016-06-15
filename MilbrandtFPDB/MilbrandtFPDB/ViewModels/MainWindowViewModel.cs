using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using System.Windows.Threading;

namespace MilbrandtFPDB
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event FileSystemEventHandler DataChanged;
        public event EventHandler<string> ErrorOccured;
        
        public const string VALUE_ANY = "(Any)";
        private Dictionary<int, SitePlan> _entries = new Dictionary<int, SitePlan>();
        private Dictionary<string, ObservableCollection<string>> _availableValues = new Dictionary<string,ObservableCollection<string>>();
        private SitePlan _selectedEntry;
        private FileSystemWatcher _fileWatcher;
        private Dispatcher _mainThread;
        private bool _preventSave;

        // Main Constructor
        public MainWindowViewModel(Dispatcher mainThreadDispatcher)
        {
            DisplayedEntries = new ObservableCollection<SitePlan>();
            LoadHeaders();
            _mainThread = mainThreadDispatcher;

            DBHelper.Type = DatabaseType.SingleFamily;
            _fileWatcher = new FileSystemWatcher(Directory.GetCurrentDirectory());
            _fileWatcher.Changed += DataFileChanged;
            LoadNewDataset();
        }

        // Test Constructor
        public MainWindowViewModel(IEnumerable<SitePlan> entries)
        {
            // don't want our unit tests messing with our data
            _preventSave = true;

            foreach (SitePlan sp in entries)
                AddEntry(sp, false);

            DisplayedEntries = new ObservableCollection<SitePlan>();
            LoadHeaders();
            ResetHeaderComboBoxes();
            RefreshDisplay();
            UpdateAvailableValues();
        }


        private void DataFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.Name != DBHelper.DataFile)
                return;

            // disable raising events again until we reload everything
            _fileWatcher.EnableRaisingEvents = false;

            if (DataChanged != null)
                DataChanged(this, e);
            
            _mainThread.Invoke((Action)(() => {
                UpdateEntries();
                RefreshDisplay();
                UpdateAvailableValues();
            }));
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

        private void LoadNewDataset()
        {
            LoadEntries();
            ResetHeaderComboBoxes();
            RefreshDisplay();
            UpdateAvailableValues();
        }

        private void LoadEntries()
        {
            _entries.Clear();
            List<SitePlan> data;

            try
            {
                data = DBHelper.Load();
                _preventSave = false;
            }
            catch (Exception ex)
            {
                OnErrorOccured("Unable to read data file:\n" + ex.Message);
                _preventSave = true; ;
                return;
            }

            foreach (SitePlan sp in data)
            {
                AddEntry(sp, false);
            }

            _fileWatcher.EnableRaisingEvents = true;
        }

        private void UpdateEntries()
        {
            try
            {
                DBHelper.Update(_entries);
                _fileWatcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                _preventSave = true;
                OnErrorOccured("A recent change made by someone else has prevented the application from updating. " +
                    "It is recomended you restart the application. " + 
                    "Any additional changes you make to this database before restarting the application will not be saved" +
                    "If this problem persists, contact an administrator." + 
                    "\n\nError Detail:\n" + ex.Message);
            }
        }

        public void SaveEntries()
        {
            if (_preventSave)
                return;

            // stop listening for changes while we save
            _fileWatcher.EnableRaisingEvents = false;
            
            // write the entries to file
            DBHelper.Write(Entries);

            // re-enable listening for changes
            _fileWatcher.EnableRaisingEvents = true;
        }

        public void AddEntry(SitePlan sp, bool refreshLists = true)
        {
            _entries.Add(sp.ID, sp);

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
            if (_entries.Remove(sp.ID))
            {
                DisplayedEntries.Remove(sp);
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
            foreach(SitePlan sp in _entries.Values)
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
            else if (_entries.Count == 0) // Edge case if something goes wrong
            {
                // Clear list of all but "Any"
                int i = 0;
                while (AvailableValues[propertyName].Count > 1)
                {
                    if (AvailableValues[propertyName][i] != VALUE_ANY)
                        AvailableValues[propertyName].RemoveAt(i);
                    else
                        i++;
                }

                // Set "Any" as the selected value
                SelectedValues[propertyName].Value = VALUE_ANY;
                return;
            }

            List<string> orderedDistinctValues;

            IComparer<string> comparer = StringComparer.CurrentCultureIgnoreCase;
            if (propertyName == "ProjectNumber")
                comparer = new ProjectNumberSort(ListSortDirection.Descending);

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

                orderedDistinctValues.Sort(comparer);

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
                    else if (comparer.Compare(_availableValues[propertyName][i], orderedDistinctValues[j]) < 0)
                    {
                        // remove one
                        _availableValues[propertyName].RemoveAt(i);
                    }
                    else if (comparer.Compare(_availableValues[propertyName][i], orderedDistinctValues[j]) > 0)
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

        public IEnumerable<SitePlan> Entries
        {
            get { return _entries.Values; }
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
                    DBHelper.Type = value;
                    OnPropertyChanged("SelectedDatabase");
                    LoadNewDataset();
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

        private void OnErrorOccured(string errorMsg)
        {
            if (ErrorOccured != null)
            {
                ErrorOccured(this, errorMsg);
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
