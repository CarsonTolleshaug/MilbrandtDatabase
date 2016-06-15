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
                DBHelper.TryToUseFile(filepath,
                    (sr) =>
                    {
                        doc = PdfDocument.Load(sr.BaseStream);
                        viewer.Document = doc;
                    },
                    10000);
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
