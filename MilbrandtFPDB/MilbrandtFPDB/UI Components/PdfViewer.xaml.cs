using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using PdfiumViewer;
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
                if (!File.Exists(_pdfFilePath))
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

    /// <summary>
    /// Interaction logic for PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : UserControl
    {
        private PdfViewerViewModel _vm;
        PdfiumViewer.PdfViewer viewer;

        public PdfViewer()
        {
            InitializeComponent();

            _vm = new PdfViewerViewModel();
            DataContext = _vm;

            viewer = new PdfiumViewer.PdfViewer();
            viewer.ShowBookmarks = false;
            ZoomMode = PdfViewerZoomMode.FitBest;
            winFormsHost.Child = viewer;
        }

        public PdfViewerZoomMode ZoomMode
        {
            get { return viewer.ZoomMode; }
            set { viewer.ZoomMode = value; }
        }

        public double Zoom
        {
            get { return viewer.Renderer.Zoom; }
            set { viewer.Renderer.Zoom = value; }
        }

        public string PdfFilePath
        {
            get { return _vm.PdfFilePath; }
            set
            {
                DrawPDF(value);
            }
        }

        private void DrawPDF(string filepath)
        {
            _vm.PdfFilePath = filepath;

            bool exists = File.Exists(filepath);
            if (!String.IsNullOrWhiteSpace(filepath) && exists)
            {
                PdfDocument doc = null;
                using (FileStream fs = File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    doc = PdfDocument.Load(fs);
                    fs.Close();
                }
                viewer.Document = doc;
                winFormsHost.Visibility = System.Windows.Visibility.Visible;
            }            
            else
            {
                winFormsHost.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public void Refresh()
        {
            DrawPDF(_vm.PdfFilePath);
        }

        public void ReleaseDocument()
        {
            PdfDocument doc = viewer.Document;
            if (doc != null)
            {
                viewer.Document = null;
                doc.Dispose();
                winFormsHost.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Refresh();
        }
    }
}
