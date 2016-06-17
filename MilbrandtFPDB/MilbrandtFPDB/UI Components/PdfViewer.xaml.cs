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
using System.IO;
using PdfiumViewer;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;

namespace MilbrandtFPDB
{
    /// <summary>
    /// Interaction logic for PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : UserControl
    {
        private string _filepath = null;

        public PdfViewer()
        {
            InitializeComponent();
            DBHelper.DocumentReleaseNeeded += DBHelper_DocumentReleaseNeeded;
        }
        ~PdfViewer()
        {
            // needed to prevent memory leaks
            DBHelper.DocumentReleaseNeeded -= DBHelper_DocumentReleaseNeeded;
        }

        private void DBHelper_DocumentReleaseNeeded(object sender, string e)
        {
            if (_filepath == e)
            {
                ReleasePDF();
            }
        }

        public void DrawPDF(string filepath, bool catchExceptions = true)
        {
            pdfPanel.IsEnabled = false;
            if (File.Exists(filepath))
            {
                try
                {
                    if (_filepath == filepath)
                    {
                        pdfBrowser.Source = null;
                    }
                    ShowViewer();
                    SetSource(filepath);
                    
                }
                catch (Exception ex)
                {
                    ReleasePDF();
                    if (!catchExceptions)
                        throw ex;
                }
            }
            else
            {
                ReleasePDF();
            }
        }

        public void ReleasePDF()
        {
            SetSource(null);
            HideViewer();
        }

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (pdfPanel.IsEnabled == false)
            {
                Point pos = e.GetPosition(pdfPanel);
                if (pos.X > 0 && pos.Y > 0 && pos.X < pdfPanel.ActualWidth && pos.Y < pdfPanel.ActualHeight)
                {
                    // Allow the pdfPanel to capture mouse input
                    pdfPanel.IsEnabled = true;

                    // Refire mouse down event
                    SendInputHelper.ClickLeftMouseButton();
                }
            }
        }

        private void HideViewer()
        {
            pdfBrowser.Visibility = System.Windows.Visibility.Hidden;
        }

        private void ShowViewer()
        {
            pdfBrowser.Visibility = System.Windows.Visibility.Visible;
        }

        private void SetSource(string file)
        {
            if (file == null)
            {
                pdfBrowser.Source = null;
            }
            else
            {
                pdfBrowser.Source = new Uri(file);
            }
            _filepath = file;
        }

        private void UserControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (pdfPanel.IsEnabled == false)
            {
                // verify its inside our control
                Point pos = e.GetPosition(pdfPanel);
                if (pos.X > 0 && pos.Y > 0 && pos.X < pdfPanel.ActualWidth && pos.Y < pdfPanel.ActualHeight)
                {
                    // Allow the pdfPanel to capture mouse input
                    pdfPanel.IsEnabled = true;
                }
            }
        }

        private void UserControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (pdfPanel.IsEnabled == false)
            {
                // verify its inside our control
                Point pos = e.GetPosition(pdfPanel);
                if (pos.X > 0 && pos.Y > 0 && pos.X < pdfPanel.ActualWidth && pos.Y < pdfPanel.ActualHeight)
                {
                    // Allow the pdfPanel to capture mouse input
                    pdfPanel.IsEnabled = true;
                }
            }
        }
    }
}
