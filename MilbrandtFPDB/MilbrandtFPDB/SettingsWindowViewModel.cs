using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

namespace MilbrandtFPDB
{
    public class SettingsWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _plansDir;
        private string _jobsFile;

        public SettingsWindowViewModel()
        {
            PlansDirectory = Settings.PlansRootDirectory;
            JobsListFile = Settings.JobListFile;
        }

        public string PlansDirectory
        {
            get { return _plansDir; }
            set
            {
                if (_plansDir != value)
                {
                    _plansDir = value;
                    OnPropertyChanged("PlansDirectory");
                }
            }
        }

        public string JobsListFile
        {
            get { return _jobsFile; }
            set
            {
                if (_jobsFile != value)
                {
                    _jobsFile = value;
                    OnPropertyChanged("JobsListFile");
                }
            }
        }

        public void Save()
        {
            if (!Directory.Exists(PlansDirectory))
                throw new ArgumentException("Cannot find plan directory path:\n" + PlansDirectory);
            if (!File.Exists(JobsListFile))
                throw new ArgumentException("Cannot find jobs list file path:\n" + JobsListFile);

            Settings.PlansRootDirectory = PlansDirectory;
            Settings.JobListFile = JobsListFile;

            Settings.SaveSettings();
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
