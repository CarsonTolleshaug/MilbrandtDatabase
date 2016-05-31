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
using System.Timers;

namespace MilbrandtFPDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataGridViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new DataGridViewModel();
            DataContext = _vm;
        }

        private void UpdatePreview(string file)
        {
            if (String.IsNullOrWhiteSpace(file) || !File.Exists(file))
            {
                pdfViewer.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                pdfViewer.Visibility = System.Windows.Visibility.Visible;
                pdfViewer.PdfFilePath = file;
            }
        }

        private void dgSitePlans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Allow multiple selections for open
            btnOpen.IsEnabled = dgSitePlans.SelectedItems.Count > 0;

            // only one item can be selected for edit or remove
            btnEdit.IsEnabled = btnRemove.IsEnabled = dgSitePlans.SelectedItems.Count == 1;

            try
            {
                UpdatePreview((dgSitePlans.SelectedItem as SitePlan).FilePath);
            }
            catch
            {
                UpdatePreview(null);
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            foreach (SitePlan sp in dgSitePlans.SelectedItems)
            {
                try
                {
                    sp.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to open file:\n" + ex.Message);
                }

            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (dgSitePlans.SelectedItems.Count != 1)
                return;

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this entry?\n(There is no undo)", "Remove Entry", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                SitePlan sp = dgSitePlans.SelectedItem as SitePlan;
                if (sp != null) // totally unneccisary, but just a precaution anyway
                {
                    _vm.RemoveEntry(sp);
                }
            }
        }

        private void dgSitePlans_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string header = e.PropertyName;

            // FilePath does not need a filter header
            if (header == "FilePath")
            {
                e.Column.Header = _vm.ParameterDisplayNames[header];
                return;
            }

            // Create the header (which is contained in a vertical stack panel)
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Vertical;

            // Add header label
            sp.Children.Add(new Label() { Content = _vm.ParameterDisplayNames[header] });

            // Create ComboBox of available filter values
            ComboBox cb = new ComboBox();
            cb.SelectionChanged += FilterSelectionChanged;
            cb.IsTabStop = false;

            // Set binding for ItemsSource
            Binding sourceBinding = new Binding("AvailableValues[" + header + "]");
            sourceBinding.Source = _vm;
            sourceBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            cb.SetBinding(ComboBox.ItemsSourceProperty, sourceBinding);
            
            // Set binding for SelectedItem
            Binding selectBinding = new Binding("SelectedValues[" + header + "].Value");
            selectBinding.Source = _vm;
            selectBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            selectBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            cb.SetBinding(ComboBox.SelectedItemProperty, selectBinding);

            // Add comboBox to header
            sp.Children.Add(cb);

            // Add header to column
            e.Column.Header = sp;
        }

        private void FilterSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _vm.RefreshDisplay();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            LaunchAddEditWizard(AddEditWizardType.Add);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            LaunchAddEditWizard(AddEditWizardType.Edit);
        }

        private void LaunchAddEditWizard(AddEditWizardType type)
        {
            AddEditWizard wizard;

            if (type == AddEditWizardType.Add)
                wizard = new AddEditWizard(type, _vm);
            else
            {
                if (dgSitePlans.SelectedItems.Count != 1)
                    return;

                wizard = new AddEditWizard(type, _vm, (SitePlan)dgSitePlans.SelectedItem);
            }

            wizard.ShowDialog();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

    }
}
