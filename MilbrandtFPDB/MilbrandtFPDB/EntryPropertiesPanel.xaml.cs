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
using System.Collections.ObjectModel;

namespace MilbrandtFPDB
{
    /// <summary>
    /// Interaction logic for EntryPropertiesPanel.xaml
    /// </summary>
    public partial class EntryPropertiesPanel : UserControl
    {
        private EntryPropertiesPanelViewModel _vm;

        public EntryPropertiesPanel()
        {
            InitializeComponent();

            _vm = new EntryPropertiesPanelViewModel();
            DataContext = _vm;

            InitializeUI();
        }

        public Dictionary<string, ObservableCollection<string>> AvailableValues
        {
            get { return _vm.AvailableValues; }
        }

        public Dictionary<string, KeyValueWrapper> PropertyValues
        {
            get { return _vm.PropertyValues; }
        }

        public Dictionary<string, KeyValueWrapper> PropertyDisplayNames
        {
            get { return _vm.PropertyDisplayNames; }
        }

        private void InitializeUI()
        {
            foreach (string property in SitePlan.Properties)
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
            TextBlock propLabel = new TextBlock();
            //propLabel.Text = PropertyDisplayNames[propertyName];
            propLabel.TextTrimming = TextTrimming.CharacterEllipsis;
            propLabel.Margin = new Thickness(0, 0, 10, 0);
            propLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            propLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            propLabel.SetValue(Grid.ColumnProperty, 0);

            // Label text binding
            Binding textBinding = new Binding("PropertyDisplayNames[" + propertyName + "].Value");
            textBinding.Source = _vm;
            propLabel.SetBinding(TextBlock.TextProperty, textBinding);

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
    }
}
