using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MilbrandtFPDB
{
    public class PdfViewerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _pdfFilePath;

        public PdfViewerViewModel()
        {
            _pdfFilePath = "";
        }

        public string PdfFilePath
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_pdfFilePath))
                    return "";
                return _pdfFilePath;
            }
            set
            {
                if (_pdfFilePath != value)
                {
                    _pdfFilePath = value;
                    OnPropertyChanged("PdfFilePath");
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
