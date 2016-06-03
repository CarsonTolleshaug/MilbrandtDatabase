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
using System.Diagnostics;

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
            _vm.PropertyChanged += VMPropertyChanged;
            DataContext = _vm;

            if (type == AddEditWizardType.Edit)
            {
                // Prevent the properties panel from being disabled 
                // due to floor plan being blank
                propertiesPanel.Style = null;
                UpdatePdfViewer();
            }

            InitializeUI();
        }

        private void InitializeUI()
        {
            foreach(string property in SitePlan.Properties)
            {
                if (property != "FilePath")
                {
                    propertiesPanel.Children.Add(GenerateElement(property));
                }
            }
        }

        private void VMPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FilePath" || e.PropertyName == "FloorPlanPath")
                UpdatePdfViewer();
        }

        private void UpdatePdfViewer()
        {
            try
            {
                pdfViewer.ReleaseDocument();
                pdfViewer.PdfFilePath = _vm.BuildTempCompositePdf();
            }
            catch 
            {
                pdfViewer.PdfFilePath = "";
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
            TextBlock propLabel = new TextBlock();
            propLabel.Text = _vm.GetParameterDisplayName(propertyName);
            propLabel.TextTrimming = TextTrimming.CharacterEllipsis;
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
                if (sender == btnBrowse)
                {
                    if (_vm.WizardType == AddEditWizardType.Add)
                    {
                        _vm.FloorPlanPath = ofd.FileName;
                        _vm.AutofillFromFloorPlan();
                    }
                    else
                    {
                        _vm.FilePath = ofd.FileName;
                    }
                }
                else
                {
                    int index = (int)((sender as Button).Tag);
                    _vm.AdditionalPdfPaths[index].Value = ofd.FileName;
                    UpdatePdfViewer();
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

        private void btnAddPdf_Click(object sender, RoutedEventArgs e)
        {
            // Create UI Element, link to vm, and add to stack panel
            
            // the index of new pdf
            int index;
            for (index = 0; index < _vm.AdditionalPdfPaths.Count && _vm.AdditionalPdfPaths[index].Value != null; index++) ;

            // Max out at 20 pdfs
            if (index >= 20)
                return;

            if (index == _vm.AdditionalPdfPaths.Count)
            {
                // Add value to vm
                _vm.AdditionalPdfPaths.Add(new KeyValueWrapper(index.ToString(), ""));
            }
            else
            {
                _vm.AdditionalPdfPaths[index].Value = "";
            }

            // Root element
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(65, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(35, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25, GridUnitType.Pixel) });
            grid.Tag = index;

            // Label
            Label label = new Label();
            label.Content = "File " + (index + 2); // start at "File 2"
            label.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            // Textbox
            TextBox tb = new TextBox();
            tb.Padding = new Thickness(1);
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            tb.SetValue(Grid.ColumnProperty, 1);
            // binding
            Binding textBinding = new Binding("AdditionalPdfPaths[" + index + "].Value");
            textBinding.Mode = BindingMode.TwoWay;
            tb.SetBinding(TextBox.TextProperty, textBinding);

            // Browse Button
            Button btn = new Button();
            btn.Content = "...";
            btn.Click += btnBrowse_Click;
            btn.Margin = new Thickness(5, 0, 5, 0);
            btn.Tag = index;
            btn.SetValue(Grid.ColumnProperty, 2);
            btn.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            // Remove Button
            Button removeBtn = new Button();
            removeBtn.Content = "X";
            removeBtn.Click += removeBtn_Click;
            removeBtn.Tag = index;
            removeBtn.SetValue(Grid.ColumnProperty, 3);
            removeBtn.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            // Add Label, Textbox, and Buttons to grid
            grid.Children.Add(label);
            grid.Children.Add(tb);
            grid.Children.Add(btn);
            grid.Children.Add(removeBtn);

            // Add grid to stack panel
            additionalPdfsPanel.Children.Insert(index, grid);
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get index from button's tag
            int index = (int)(sender as Button).Tag;
            
            // Clear the path value so it isn't used
            _vm.AdditionalPdfPaths[index].Value = null;

            // Find the parent grid and remove it from the stack panel
            foreach(UIElement child in additionalPdfsPanel.Children)
            {
                Grid grid = (child as Grid);
                int? tag = grid == null ? null : (grid.Tag as int?);
                if (tag.HasValue && tag.Value == index)
                {
                    additionalPdfsPanel.Children.Remove(child);

                    // return so we don't get an exception for modifying the collection
                    return; 
                }
            }

        }

        private void btnOpenPDF_Click(object sender, RoutedEventArgs e)
        {
            // Open the PDF in Acrobat
            Process open = new Process();
            if (_vm.WizardType == AddEditWizardType.Add)
                open.StartInfo = new ProcessStartInfo(_vm.FloorPlanPath);
            else
                open.StartInfo = new ProcessStartInfo(_vm.FilePath);

            try
            {
                open.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open file");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            pdfViewer.ReleaseDocument();
        }
    }
}
