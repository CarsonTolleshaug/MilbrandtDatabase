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
using System.Threading;
using System.Diagnostics;

namespace MilbrandtFPDB
{
    /// <summary>
    /// Interaction logic for PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : UserControl
    {
        private PdfViewerViewModel _vm;

        private const double ZOOM_SMALL_COEF = 0.001;
        private const double ZOOM_LARGE_COEF = 0.05;
        private readonly int MARGIN;
        private readonly int SCROLL_OFFSET;

        private CancellationTokenSource tokenSource;
        private Process currentProcess = Process.GetCurrentProcess();
        private PdfDocument pdfDoc;
        private List<BitmapSource> _pages;

        public PdfViewer()
        {
            InitializeComponent();

            _vm = new PdfViewerViewModel();
            DataContext = _vm;
            _vm.PropertyChanged += _vm_PropertyChanged;

            _pages = new List<BitmapSource>();

            scrollBar.Minimum = 0;
            MARGIN = (int)imgDisplay.Margin.Bottom;
            SCROLL_OFFSET = (MARGIN * 2) + 19;
        }

        public string PdfFilePath
        {
            get { return _vm.PdfFilePath; }
            private set
            {
                _vm.PdfFilePath = value;
            }
        }

        public double Zoom
        {
            get { return _vm.Zoom; }
            set { _vm.Zoom = value; }
        }

        public void Dispose()
        {
            if (tokenSource != null)
                tokenSource.Cancel();

            if (pdfDoc != null)
                pdfDoc.Dispose();

            _vm.Dispose();
        }

        private void HideViewer()
        {
            imgDisplay.Visibility = System.Windows.Visibility.Hidden;
            previewUnavailableText.Visibility = System.Windows.Visibility.Visible;
        }

        private void ShowViewer()
        {
            imgDisplay.Visibility = System.Windows.Visibility.Visible;
            previewUnavailableText.Visibility = System.Windows.Visibility.Hidden;
        }

        public async Task DrawPDF(string filepath)
        {
            PdfFilePath = filepath;

            MemoryStream ms = await _vm.GetPdfMemStreamAsync();
            if (ms != null)
            {
                try
                {
                    pdfDoc = PdfDocument.Load(ms);
                    if (pdfDoc != null)
                    {
                        await RenderPdfDoc();
                        ShowViewer();
                    }
                    else
                        HideViewer();
                }
                catch
                {
                    HideViewer();
                }
            }
            else
            {
                HideViewer();
            }
        }

        private async void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await DrawPDF(PdfFilePath);
        }

        private async Task RenderPdfDoc()
        {
            
            imgDisplay.Source = null;
            _pages.Clear();

            try
            {
                for (int i = 0; i < pdfDoc.PageCount; i++)
                {
                    _pages.Add(
                        await
                            Task.Run<BitmapSource>(
                                new Func<BitmapSource>(
                                    () =>
                                    {
                                        tokenSource.Token.ThrowIfCancellationRequested();

                                        return RenderPageToMemDC(i, 
                                            (int)pdfDoc.PageSizes[i].Width, 
                                            (int)pdfDoc.PageSizes[i].Height);
                                    }
                            ), tokenSource.Token));

                    if (_pages.Count == 1)
                    {
                        imgDisplay.Source = _pages[0];
                        _vm.PageCount = pdfDoc.PageCount;
                        _vm.PageNumber = 1;
                        Zoom = Math.Min((hScrollViewer.ActualHeight - MARGIN) / imgDisplay.Source.Height,
                            (hScrollViewer.ActualWidth - MARGIN) / imgDisplay.Source.Width);
                    }

                    currentProcess.Refresh();

                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                tokenSource.Cancel();
                _pages.Clear();
                MessageBox.Show(ex.Message);
            }

            scrollBar.Value = 0;
            scrollBar.Maximum = (double)_pages.Count - scrollBar.SmallChange;
        }

        private BitmapSource RenderPageToMemDC(int page, int width, int height)
        {
            var image = pdfDoc.Render(page, width, height, 96, 96, false);
            return BitmapHelper.ToBitmapSource(image);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tokenSource = new CancellationTokenSource();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {        
            Dispose();
        }

        private void scrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_pages != null && _pages.Count > 0)
            {
                double value = e.NewValue;
                int intValue = (int)e.NewValue;

                int page = Math.Min(intValue, _pages.Count - 1);
                imgDisplay.Source = _pages[page];
                _vm.PageNumber = page + 1;

                // if its the start of a new page
                if ((int)e.OldValue < intValue)
                    scrollBar.Value = intValue;
                else if ((int)e.OldValue > intValue)
                    scrollBar.Value = (int)e.OldValue - scrollBar.SmallChange;
                else
                {
                    // scroll to an offset of the remainder (after decimal place) of the scroll bar value
                    hScrollViewer.ScrollToVerticalOffset(((value - intValue) * (imgDisplay.ActualHeight - (hScrollViewer.ActualHeight - SCROLL_OFFSET))));
                }
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(PdfFilePath))
            {
                Process openProc = new Process();
                openProc.StartInfo = new ProcessStartInfo(PdfFilePath);
                openProc.Start();
            }
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            Zoom -= ZOOM_LARGE_COEF;
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            Zoom += ZOOM_LARGE_COEF;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                // Zoom
                Zoom += e.Delta * ZOOM_SMALL_COEF;
                e.Handled = true;
            }
            else if (Keyboard.IsKeyDown(Key.LeftAlt))
            {
                // Scroll horizontally
                hScrollViewer.ScrollToHorizontalOffset(hScrollViewer.HorizontalOffset - e.Delta);
                e.Handled = true;
            }
            else
            {
                // Scroll vertically
                scrollBar.Value -= e.Delta * scrollBar.SmallChange;
                e.Handled = true;
            }
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Zoom")
            {
                ApplyZoom();
            }
        }

        private void ApplyZoom()
        {
            if (imgDisplay.Source != null)
            {
                imgDisplay.Width = imgDisplay.Source.Width * Zoom;
                imgDisplay.Height = imgDisplay.Source.Height * Zoom;
            }
        }

        private void hScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            UserControl_MouseWheel(sender, e);
        }

        private void hScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            UserControl_MouseWheel(sender, e);
        }
    }
}
