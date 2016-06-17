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
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Controls.Primitives;

namespace MilbrandtFPDB
{
    /// <summary>
    /// ************************ MAIN WINDOW ************************
    /// Contains the main UI elements for the application. Provides
    /// ability to launch other windows. UI Elements include:
    ///     + DataGrid
    ///         - Contains the list of site plan entries, manages
    ///           filtering list via combo boxes in headers
    ///     + Pdf Viewer
    ///         - Provides a preview of the currently selected site
    ///           plan's PDF file
    ///     + Menu Items
    ///         - Add Entry (launches AddEditWizard)
    ///         - Edit Entry (launches AddEditWizard if only one item
    ///           is selected, otherwise launches BatchEditWizard)
    ///         - Remove Entry
    ///         - Change Database (combo box)
    ///         - Open PDF (opens the currently displayed previewing
    ///           pdf file in default external app)
    ///         - Settings (launches SettingsWindow)
    /// The code here is simply to handle completely UI level things 
    /// (like buttons being enabled) or to create UI elements. All 
    /// the logic code is contained either in the view model 
    /// (MainWindowViewModel) or the model (SitePlan)
    /// *************************************************************
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _vm;
        private int _generatedColumns = 0;
        private IInputElement _lastFocus = null;
        private List<SitePlan> _lastSelection = new List<SitePlan>();
        private bool _lastFocusInDataGrid = false;

        public MainWindow()
        {
            InitializeComponent();

            _vm = new MainWindowViewModel(this.Dispatcher);
            DataContext = _vm;
            _vm.ErrorOccured += VM_ErrorOccured;
            _vm.DisplayListChanging += VM_DisplayListChanging;
            _vm.DisplayListChanged += VM_DisplayListChanged;
        }

        #region DataGrid

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Allow multiple selections for open & edit
            btnEdit.IsEnabled = dgSitePlans.SelectedItems.Count > 0;

            // only one item can be selected for remove
            btnRemove.IsEnabled = dgSitePlans.SelectedItems.Count == 1;

            if (dgSitePlans.SelectedItems.Count > 1)
                btnEditText.Text = "Edit Entries";
            else
                btnEditText.Text = "Edit Entry";

            UpdatePreview();
        }


        /// <summary>
        /// This is called when the data grid begins generating columns for 
        /// the public properties of SitePlan. This handler gives us control
        /// over the creation of these columns and allows us to add the
        /// filter combo boxes in the header, as well as other neccessary
        /// tweaks.
        /// </summary>
        private void DataGridAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // the name of the column
            string header = e.PropertyName;

            // FilePath should not have a column
            if (header == "FilePath")
            {
                e.Cancel = true;
                return;
            }

            if (header == "Date")
            {
                DataGridTextColumn newCol = new DataGridTextColumn();
                newCol.Binding = new Binding("Date") { StringFormat = "{0:MM/dd/yyyy}" };
                e.Column = newCol;
            }

            // Setting Right-Justify for square feet
            if (header == "SquareFeet")
            {
                DataGridTextColumn newCol = new DataGridTextColumn();

                newCol.Binding = new Binding("SquareFeet");

                Style sqftStyle = new Style(typeof(TextBlock));
                sqftStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
                sqftStyle.Setters.Add(new Setter(TextBlock.PaddingProperty, new Thickness(1, 1, 8, 1)));
                newCol.ElementStyle = sqftStyle;

                e.Column = newCol;
            }

            // Create the header (which is contained in a vertical stack panel)
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Vertical;
            sp.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            sp.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            sp.Tag = header;

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

            // Set binding for drop down height
            Binding ddhBinding = new Binding("ActualHeight");
            ddhBinding.Source = dgSitePlans;
            ddhBinding.Converter = new MilbrandtFPDB.MathConverter();
            ddhBinding.ConverterParameter = "(@VALUE - 60)";
            ddhBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            cb.SetBinding(ComboBox.MaxDropDownHeightProperty, ddhBinding);

            // Add comboBox to header
            sp.Children.Add(cb);

            // Add header to column
            e.Column.Header = sp;
            e.Column.MinWidth = 65;

            // Setting Column Width
            while (MilbrandtFPDB.Properties.Settings.Default.ColumnWidths.Count <= _generatedColumns)
                MilbrandtFPDB.Properties.Settings.Default.ColumnWidths.Add("Auto");
            e.Column.Width = StringToColumnWidthConverter.ConvertToWidth(MilbrandtFPDB.Properties.Settings.Default.ColumnWidths[_generatedColumns]);

            // Setting ComboBox Color (Filtering)
            Style colHeaderStyle = new System.Windows.Style(typeof(ComboBox), cb.Style);
            Binding filterBinding = new Binding("SelectedValues[" + header + "].Value");
            filterBinding.Source = _vm;
            filterBinding.Converter = new ValueToIsAnyBool();
            DataTrigger filterTrigger = new DataTrigger();
            filterTrigger.Binding = filterBinding;
            filterTrigger.Value = false;
            filterTrigger.Setters.Add(new Setter(ComboBox.BackgroundProperty, (LinearGradientBrush)FindResource("ColumnFilterBrush")));
            colHeaderStyle.Triggers.Add(filterTrigger);
            cb.Template = (ControlTemplate)FindResource("DataGridHeaderComboBoxStyle");
            cb.Style = colHeaderStyle;

            

            // Set default sorting
            if (header == "ProjectNumber")
                SortDataGridByProjectNumber(ListSortDirection.Descending, e.Column);

            _generatedColumns++;
        }


        /// <summary>
        /// This is called when one of the filters is changed. We let the view model
        /// handle the filtering, we just need to tell it to refresh.
        /// </summary>
        private void FilterSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _vm.RefreshDisplay();
        }


        /// <summary>
        /// This is called when the user clicks the header of a column in the
        /// DataGrid to sort the entries by that column. This handler gives
        /// us the ability to alter the sorting of ProjectNumber. Because the
        /// project number's first two digits are the last two digits of the
        /// year, we want to display those which occur in 1900's to come before
        /// the ones in the 2000's. Ex: 9810 should come before 0310
        /// </summary>
        private void DataGridSorting(object sender, DataGridSortingEventArgs e)
        {
            string header = (e.Column.Header as StackPanel).Tag.ToString();
            if (header == "ProjectNumber")
            {
                // prevent the built-in sort from sorting
                e.Handled = true;

                ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;
                SortDataGridByProjectNumber(direction, e.Column);
            }
        }


        /// <summary>
        /// This is a helper method used to sort the DataGrid entries by ProjectNumber
        /// using a custom comparer to accomplish the sorting order we want.
        /// </summary>
        /// <param name="direction">The direction to sort (Ascending/Decending)</param>
        /// <param name="col">A reference to the ProjectNumber column</param>
        private void SortDataGridByProjectNumber(ListSortDirection direction, DataGridColumn col)
        {
            System.Collections.IComparer comparer = null;

            //use a ListCollectionView to do the sort.
            ListCollectionView lcv = (ListCollectionView)CollectionViewSource.GetDefaultView(dgSitePlans.ItemsSource);

            col.SortDirection = direction;
            comparer = new ProjectNumberSort(direction);

            //apply the sort
            lcv.CustomSort = comparer;
        }



        private static DataGridCell GetCell(DataGrid dataGrid, DataGridRow rowContainer, int column)
        {
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GeneralHelpers.GetChildOfType<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    /* if the row has been virtualized away, call its ApplyTemplate() method
                     * to build its visual tree in order for the DataGridCellsPresenter
                     * and the DataGridCells to be created */
                    rowContainer.ApplyTemplate();
                    presenter = GeneralHelpers.GetChildOfType<DataGridCellsPresenter>(rowContainer);
                }
                if (presenter != null)
                {
                    DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    if (cell == null)
                    {
                        /* bring the column into view
                         * in case it has been virtualized away */
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                        cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    }
                    return cell;
                }
            }
            return null;
        }


        #endregion


        #region Pdf Viewer

        private void UpdatePreview()
        {
            if (_vm.SelectedEntry != null)
                pdfViewer.DrawPDF(_vm.SelectedEntry.FilePath);
            else
                pdfViewer.DrawPDF(null);
        }

        #endregion


        #region Menu Items

        private void btnOpenPDF_Click(object sender, RoutedEventArgs e)
        {
            foreach (SitePlan sp in dgSitePlans.SelectedItems)
            {
                try
                {
                    DBHelper.RequestDocumentRelease(sp.FilePath);
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
                    _vm.SaveEntries();
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            LaunchAddEditWizard(AddEditWizardType.Add);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            LaunchAddEditWizard(AddEditWizardType.Edit);
        }

        private bool? LaunchAddEditWizard(AddEditWizardType type)
        {
            Window wizard;

            if (type == AddEditWizardType.Add)
                wizard = new AddEditWizard(type, _vm);
            else
            {
                if (dgSitePlans.SelectedItems.Count == 0)
                    return null;
                else if (dgSitePlans.SelectedItems.Count > 1)
                    wizard = new BatchEditWizard(_vm, dgSitePlans.SelectedItems.Cast<SitePlan>());
                else
                    wizard = new AddEditWizard(type, _vm, (SitePlan)dgSitePlans.SelectedItem);
            }

            wizard.Owner = this;

            bool? result = wizard.ShowDialog();

            dgSitePlans.Focus();
            if (result.HasValue && result.Value)
            {
                if (_vm.SelectedEntry != null)
                {
                    dgSitePlans.ScrollIntoView(_vm.SelectedEntry);
                }
                _vm.SaveEntries();
            }

            UpdatePreview();

            // put keyboard focus on the selected cell
            if (dgSitePlans.SelectedIndex >= 0)
            {
                DataGridRow row = dgSitePlans.ItemContainerGenerator.ContainerFromIndex(dgSitePlans.SelectedIndex) as DataGridRow;
                if (row != null)
                {
                    DataGridCell cell = GetCell(dgSitePlans, row, 0);
                    if (cell != null)
                        cell.Focus();
                }
            }

            return result;
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow() { Owner = this };
            bool? result = settingsWindow.ShowDialog();
            if (result.HasValue && result.Value)
            {
                // because Square feet range step value may have changed
                _vm.UpdateAvailableValues("SquareFeet");
            }
        }

        #endregion


        private void VM_ErrorOccured(object sender, string e)
        {
            MessageBox.Show(e);
        }

        private void VM_DisplayListChanging(object sender, EventArgs e)
        {
            // store last focus and selected
            _lastFocus = Keyboard.FocusedElement;
            _lastSelection.Clear();
            _lastSelection.AddRange(dgSitePlans.SelectedItems.Cast<SitePlan>());
            _lastFocusInDataGrid = dgSitePlans.IsFocused || dgSitePlans.IsKeyboardFocused || dgSitePlans.IsKeyboardFocusWithin;
        }

        private void VM_DisplayListChanged(object sender, EventArgs e)
        {
            // restore focus and selection
            if (_lastFocusInDataGrid)
            {
                dgSitePlans.Focus();
                _lastFocusInDataGrid = false;
            }
            foreach (SitePlan sp in _lastSelection)
                dgSitePlans.SelectedItems.Add(sp);
            _lastSelection.Clear();

            if (_lastFocus != null)
            {
                _lastFocus.Focus();
                DataGridCell cell = _lastFocus as DataGridCell;
                if (cell != null)
                    cell.Focus();
                _lastFocus = null;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // save column widths
            for (int i = 0; i < dgSitePlans.Columns.Count; i++)
            {
                MilbrandtFPDB.Properties.Settings.Default.ColumnWidths[i] =
                    StringToColumnWidthConverter.ConvertToString(dgSitePlans.Columns[i].Width);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
                _vm.RefreshDisplay();
        }
    }
}
