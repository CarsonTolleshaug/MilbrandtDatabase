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
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Win32;

namespace MilbrandtFPDB
{
    /// <summary>
    /// Interaction logic for AddEditWizard.xaml
    /// </summary>
    public partial class AddEditWizard : Window
    {
        private AddEditWizardViewModel _vm;

        public AddEditWizard(AddEditWizardType type, DataGridViewModel parentVM, SitePlan entry = null)
        {
            InitializeComponent();

            // 
            if (type == AddEditWizardType.Edit)
                propertiesPanel.Style = null;

            _vm = new AddEditWizardViewModel(type, parentVM, entry);
            DataContext = _vm;

            InitializeUI();
        }

        private void InitializeUI()
        {
            foreach(string property in SitePlan.Parameters)
            {
                if (property != "FilePath")
                {
                    propertiesPanel.Children.Add(GenerateElement(property));
                }
            }
        }

        private UIElement GenerateElement(string propertyName)
        {
            // Create grid to be our root element
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
            grid.Margin = new System.Windows.Thickness(0, 0, 20, 4);

            // Create label on left side to show property name
            Label propLabel = new Label();
            propLabel.Content = _vm.GetParameterDisplayName(propertyName);
            propLabel.Margin = new Thickness(0, 0, 10, 0);
            propLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            propLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            propLabel.SetValue(Grid.ColumnProperty, 0);

            // Add Label to grid
            grid.Children.Add(propLabel);

            // Special Date Selector
            if (propertyName == "Date")
            {
                DatePicker dp = new DatePicker();
                dp.SelectedDateChanged += DatePickerDateChanged;
                dp.SetValue(Grid.ColumnProperty, 1);
                dp.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                // Set binding for Text
                Binding selectBinding = new Binding("PropertyValues[Date].Value");
                selectBinding.Source = _vm;
                selectBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
                selectBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                dp.SetBinding(DatePicker.TextProperty, selectBinding);

                // Add DatePicker to grid
                grid.Children.Add(dp);
            }
            else
            {
                ComboBox propCB = new ComboBox();
                propCB.IsEditable = true;
                propCB.SetValue(Grid.ColumnProperty, 1);
                propCB.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                // Set binding for ItemsSource
                Binding sourceBinding = new Binding("AvailableValues[" + propertyName + "]");
                sourceBinding.Source = _vm;
                sourceBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                propCB.SetBinding(ComboBox.ItemsSourceProperty, sourceBinding);

                // Set binding for Text (SelectedItem)
                Binding selectBinding = new Binding("PropertyValues[" + propertyName + "].Value");
                selectBinding.Source = _vm;
                selectBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
                selectBinding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
                propCB.SetBinding(ComboBox.TextProperty, selectBinding);

                // combo box to grid            
                grid.Children.Add(propCB);
            }

            return grid;
        }

        private void DatePickerDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker dp = (sender as DatePicker);
            if (dp != null && dp.SelectedDate.HasValue)
                _vm.PropertyValues["Date"].Value = dp.SelectedDate.Value.ToShortDateString();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF files (*.pdf, *.pdfx)|*.pdf;*.pdfx|All files (*.*)|*.*";

            bool? result = ofd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                if (sender == btnFilePathBrowse)
                {
                    _vm.FilePath = ofd.FileName;
                }
                else if (sender == btnFloorPlanBrowse)
                {
                    _vm.FloorPlanPath = ofd.FileName;
                    _vm.AutofillFromFloorPlan();
                }
                else if (sender == btnElevationBrowse)
                {
                    _vm.ElevationPath = ofd.FileName;
                }
            }
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
    }
}
