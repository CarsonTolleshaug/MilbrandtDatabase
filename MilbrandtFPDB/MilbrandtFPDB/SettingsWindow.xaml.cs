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
using System.Windows.Shapes;
using Microsoft.Win32;

namespace MilbrandtFPDB
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private SettingsWindowViewModel _vm;

        public SettingsWindow()
        {
            InitializeComponent();
            _vm = new SettingsWindowViewModel();
            DataContext = _vm;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _vm.Save();
                this.DialogResult = true;
                this.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save:\n" + ex.Message);
            }
        }

        private void btnBrowsePlansDir_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = fbd.ShowDialog(this.GetIWin32Window());

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _vm.PlansDirectory = fbd.SelectedPath;
            }
        }

        private void btnBrowseJobsFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = @"Job List Data file (jobs.dat)|jobs.dat";

            bool? result = ofd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                _vm.JobsListFile = ofd.FileName;
            }
        }
    }


    // needed for the folder browser dialog
    public static class MyWpfExtensions
    {
        public static System.Windows.Forms.IWin32Window GetIWin32Window(this System.Windows.Media.Visual visual)
        {
            var source = System.Windows.PresentationSource.FromVisual(visual) as System.Windows.Interop.HwndSource;
            System.Windows.Forms.IWin32Window win = new OldWindow(source.Handle);
            return win;
        }

        private class OldWindow : System.Windows.Forms.IWin32Window
        {
            private readonly System.IntPtr _handle;
            public OldWindow(System.IntPtr handle)
            {
                _handle = handle;
            }

            #region IWin32Window Members
            System.IntPtr System.Windows.Forms.IWin32Window.Handle
            {
                get { return _handle; }
            }
            #endregion
        }
    }
}
