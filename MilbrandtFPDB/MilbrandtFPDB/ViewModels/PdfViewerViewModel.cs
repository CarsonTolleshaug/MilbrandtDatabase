﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

namespace MilbrandtFPDB
{
    public class PdfViewerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private const int READ_TIMEOUT = 5000;
        private const double ZOOM_MAX = 5.0;

        private string _pdfFilePath;
        private MemoryStream _memStream;
        private double _zoom;
        private int _pageNumber;
        private int _pageCount;

        public PdfViewerViewModel()
        {
            _pdfFilePath = "";
            Zoom = 1;
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

        public double Zoom
        {
            get { return _zoom; }
            set 
            { 
                if (_zoom != value && value > 0 && value <= ZOOM_MAX)
                {
                    _zoom = value;
                    OnPropertyChanged("Zoom");
                }
            }
        }

        public int PageNumber
        {
            get 
            {
                return _pageNumber;
            }
            set
            {
                if (value != _pageNumber && value >= 0 && value <= PageCount)
                {
                    _pageNumber = value;
                    OnPropertyChanged("PageNumber");
                }
            }
        }

        public int PageCount
        {
            get { return _pageCount; }
            set
            {
                if (_pageCount != value && value >= 0)
                {
                    _pageCount = value;
                    OnPropertyChanged("PageCount");
                }
            }
        }

        public async Task<MemoryStream> GetPdfMemStreamAsync()
        {
            if (_memStream != null)
            {
                _memStream.Close();
                _memStream.Dispose();
                _memStream = null;
            }

            PageNumber = 0;
            PageCount = 0;

            if (File.Exists(PdfFilePath))
            {
                bool success = await DBHelper.TryToUseFileAsync(PdfFilePath, async (sr) =>
                {
                    _memStream = new MemoryStream();
                    await sr.BaseStream.CopyToAsync(_memStream);
                    sr.Close();
                },
                READ_TIMEOUT);

                return _memStream;
            }
            return null;
        }

        public void Dispose()
        {
            if (_memStream != null)
            {
                _memStream.Close();
                _memStream.Dispose();
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
