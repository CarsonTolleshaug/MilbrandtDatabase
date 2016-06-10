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

namespace MilbrandtFPDB
{
    /// <summary>
    /// Interaction logic for PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : UserControl
    {
        private string _pdfFilePath;
        PdfiumViewer.PdfViewer viewer;

        public PdfViewer()
        {
            InitializeComponent();

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
            get { return _pdfFilePath; }
            set
            {
                DrawPDF(value);
            }
        }

        private void DrawPDF(string filepath)
        {
            _pdfFilePath = filepath;

            bool exists = File.Exists(filepath);
            if (!String.IsNullOrWhiteSpace(filepath) && exists)
            {
                PdfDocument doc = PdfDocument.Load(filepath);
                viewer.Document = doc;
                winFormsHost.Visibility = System.Windows.Visibility.Visible;
            }            
            else
            {
                winFormsHost.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public void ReleaseDocument()
        {
            PdfDocument doc = viewer.Document;
            if (doc != null)
            {
                viewer.Document = null;
                doc.Dispose();
            }
        }
    }
}
