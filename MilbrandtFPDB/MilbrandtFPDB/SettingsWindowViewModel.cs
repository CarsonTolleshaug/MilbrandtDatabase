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
        private string _sqftRange;

        public SettingsWindowViewModel()
        {
            PlansDirectory = Settings.PlansRootDirectory;
            JobsListFile = Settings.JobListFile;
            SqftRangeStep = Settings.SqftRangeStep.ToString();
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

        public string SqftRangeStep
        {
            get { return _sqftRange; }
            set
            {
                int temp;
                if (_sqftRange != value && int.TryParse(value, out temp) && temp >= 0)
                {
                    _sqftRange = value;
                    OnPropertyChanged("SqftRangeStep");
                }
                else if (_sqftRange != value && string.IsNullOrWhiteSpace(value))
                {
                    _sqftRange = "";
                    OnPropertyChanged("SqftRangeStep");
                }
            }
        }

        public void Save()
        {
            if (!Directory.Exists(PlansDirectory))
                throw new ArgumentException("Cannot find plan directory path:\n" + PlansDirectory);
            if (!File.Exists(JobsListFile))
                throw new ArgumentException("Cannot find jobs list file path:\n" + JobsListFile);

            if (String.IsNullOrWhiteSpace(SqftRangeStep))
                throw new ArgumentException("Square Ft. Range Step Value cannot be blank");
            int temp;
            if (!int.TryParse(SqftRangeStep, out temp) || temp <= 0)
                throw new ArgumentException("Square Ft. Range Step Value must be a valid positive integer");

            Settings.PlansRootDirectory = PlansDirectory;
            Settings.JobListFile = JobsListFile;
            Settings.SqftRangeStep = temp;

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
