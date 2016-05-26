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
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
            grid.Margin = new System.Windows.Thickness(0, 0, 20, 4);

            Label propLabel = new Label();
            propLabel.Content = _vm.GetParameterDisplayName(propertyName);
            propLabel.Margin = new Thickness(0, 0, 10, 0);
            propLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            propLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            propLabel.SetValue(Grid.ColumnProperty, 0);

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

            // Add label and combo box to grid and return it
            grid.Children.Add(propLabel);
            grid.Children.Add(propCB);

            return grid;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF files (*.pdf, *.pdfx)|*.pdf;*.pdfx|All files (*.*)|*.*";

            bool? result = ofd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                _vm.FilePath = ofd.FileName;
                _vm.AutofillFromFilePath();
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
