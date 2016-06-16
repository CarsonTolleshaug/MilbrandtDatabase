using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MilbrandtFPDB
{
    public class EntryPropertiesPanelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public EntryPropertiesPanelViewModel()
        {
            AvailableValues = new Dictionary<string, ObservableCollection<string>>();
            PropertyValues = new Dictionary<string, KeyValueWrapper>();
            PropertyDisplayNames = new Dictionary<string, KeyValueWrapper>();

            foreach (string property in SitePlan.Properties)
            {
                if (property != "FilePath")
                {
                    AvailableValues[property] = new ObservableCollection<string>();
                    PropertyValues[property] = new KeyValueWrapper(property, "");
                }

                PropertyDisplayNames[property] = new KeyValueWrapper(property, property);
            }
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

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
