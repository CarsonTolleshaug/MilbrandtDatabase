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

namespace MilbrandtFPDB
{
    /// <summary>
    /// Interaction logic for BatchEditWizard.xaml
    /// </summary>
    public partial class BatchEditWizard : Window
    {
        private BatchEditWizardViewModel _vm;

        public BatchEditWizard(MainWindowViewModel parentVM, IEnumerable<SitePlan> entries)
        {
            InitializeComponent();

            _vm = new BatchEditWizardViewModel(parentVM, entries, propertiesPanel.AvailableValues, propertiesPanel.PropertyValues, propertiesPanel.PropertyDisplayNames);
            _vm.ErrorOccured += VMErrorOccured;
            DataContext = _vm;

            propertiesPanel.DatePickerWatermark = BatchEditWizardViewModel.VALUE_VARIED;
        }

        private void VMErrorOccured(object sender, string e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.Close();
            }));

            MessageBox.Show(e);
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _vm.RaiseErrorOnPropertyChanged = false;
        }
    }
}
