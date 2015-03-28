using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PlanReferenceDatabase
{
    public enum FormMode { Search, Edit }

    public partial class Form1 : Form
    {
        List<SitePlan> _display; //the list to keep track of whats displayed in the data grid view (not static; changes with sort order)
        List<SitePlan> _sites; //the actual (somewhat static) list of sites.
        bool saved = false;
        bool listPopulating = false;
        FormMode mode;
        OpenFileDialog fileDialog;

        public Form1()
        {
            InitializeComponent();
            LoadLayout();
            LoadData();
            UpdateLists();
            ChangeLayout(FormMode.Search);
            UpdateColumnWidths();
            fileDialog = new OpenFileDialog();
        }

        #region Utility Methods
        private void LoadData()
        {
            try
            {
                //attempts to read the list from file
                _sites = DataBase.Read();
            }
            catch
            {
                MessageBox.Show("Cannot Read File.");
                _sites = new List<SitePlan>();
            }
            //initializes _display regaurdless of whether the read was succcessful or not
            _display = new List<SitePlan>(_sites);
            SitePlan.Sort = SortMethod.Default;
        }
        private bool SaveData()
        {
            try
            {
                //attempts to save the list to file
                DataBase.Write(_sites);
                saved = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save Failed. " + ex.Message);
                return false;
            }
        }
        private void LoadLayout()
        {
            //each property has an index in the array of strings
            string[] settings = DataBase.ReadLayout();
            try
            {
                //checks whether or not the window
                //should be maximized:
                if (bool.Parse(settings[0]))
                    this.WindowState = FormWindowState.Maximized;
                else
                {
                    this.Location = new Point(int.Parse(settings[1]), int.Parse(settings[2]));
                    this.Width = int.Parse(settings[3]);
                    this.Height = int.Parse(settings[4]);
                }
                DataBase.Type = (DatabaseType)int.Parse(settings[5]);
                splitContainer.SplitterDistance = int.Parse(settings[6]);
                checkPreview.Checked = bool.Parse(settings[7]);
                for (int i = 0; i < dgList.ColumnCount; i++)
                    dgList.Columns[i].Width = int.Parse(settings[i + 8]);
            }
            catch 
            {
                //No longer neccisary due to the StartForm
                //DataBase.Type = DatabaseType.Flat;
                checkPreview.Checked = true;
            }

            if (!checkPreview.Checked)
                UpdatePreview("");
            lblName.Text = "Database: " + DataBase.Type.ToString();
        }
        private void SaveLayout()
        {
            string[] settings = new string[19];
            settings[0] = (this.WindowState == FormWindowState.Maximized).ToString();
            settings[1] = this.Left.ToString();
            settings[2] = this.Top.ToString();
            settings[3] = this.Width.ToString();
            settings[4] = this.Height.ToString();
            settings[5] = ((int)DataBase.Type).ToString();
            settings[6] = splitContainer.SplitterDistance.ToString();
            settings[7] = checkPreview.Checked.ToString();
            for (int i = 0; i < dgList.ColumnCount; i++)
                settings[i + 8] = dgList.Columns[i].Width.ToString();
            DataBase.WriteLayout(settings);
        }
        /// <summary>
        /// Updates the DataGridView to 
        /// reflect any changes in the
        /// list of site plans, and
        /// Updates the combo boxes to
        /// contain all existing entry
        /// options.
        /// </summary>
        private void UpdateLists()
        {
            saved = false;
            //Populating Combo Boxes
            foreach (SitePlan s in _sites)
            {
                if (!cbProjNum.Items.Contains(s.ProjNumber))
                    cbProjNum.Items.Add(s.ProjNumber);
                if (!cbProjName.Items.Contains(s.ProjName))
                    cbProjName.Items.Add(s.ProjName);
                if (!cbClientName.Items.Contains(s.ClientName))
                    cbClientName.Items.Add(s.ClientName);
                if (!cbLocation.Items.Contains(s.Location))
                    cbLocation.Items.Add(s.Location);
                if (!cbPlan.Items.Contains(s.Plan))
                    cbPlan.Items.Add(s.Plan);
                if (!cbWidth.Items.Contains(s.Width))
                    cbWidth.Items.Add(s.Width);
                if (!cbDepth.Items.Contains(s.Depth))
                    cbDepth.Items.Add(s.Depth);
                if (!cbBeds.Items.Contains(s.Bedrooms))
                    cbBeds.Items.Add(s.Bedrooms);
                if (!cbBaths.Items.Contains(s.Baths))
                    cbBaths.Items.Add(s.Baths);
            }

            //prevents the preview from updating 
            //for every new row.
            listPopulating = true;

            //Populating Data Grid View
            if (_display.Count != 0)
            {
                dgList.RowCount = _display.Count;
                for (int i = 0; i < _display.Count; i++)
                {
                    SitePlan s = _display[i];
                    dgList.Rows[i].SetValues(DataBase.WriteProjectNumber(s.ProjNumber), s.ProjName, s.ClientName,
                        s.Location, s.Plan, s.Width, s.Depth, s.Bedrooms, s.Baths, s.Sqrft, s.Date);
                    dgList.Rows[i].Tag = s;
                }
            }
            else
            {
                dgList.RowCount = 1;
                dgList.Rows[0].SetValues("", "", "", "", "", "", "", "", "", "", "");
            }

            listPopulating = false;
        }
        /// <summary>
        /// Switches the layout of the form
        /// between "search mode" and "edit
        /// mode".
        /// </summary>
        private void ChangeLayout(FormMode disiredMode)
        {
            int selectedRow = 0;
            if (dgList.SelectedRows.Count > 0)
                 selectedRow = dgList.SelectedRows[0].Index;

            this.mode = disiredMode;
            lblToolTip.Visible = mode == FormMode.Search;
            searchModeControls.Enabled = mode == FormMode.Search;
            dgList.MultiSelect = mode == FormMode.Search;
            btnOpen.Enabled = mode == FormMode.Search;
            editModeControls.Enabled = mode == FormMode.Edit;

            if (disiredMode == FormMode.Edit)
            {
                cbProjNum.DropDownStyle =
                    cbProjName.DropDownStyle =
                    cbClientName.DropDownStyle =
                    cbLocation.DropDownStyle =
                    cbPlan.DropDownStyle =
                    cbWidth.DropDownStyle =
                    cbDepth.DropDownStyle = 
                    cbBeds.DropDownStyle =
                    cbBaths.DropDownStyle = ComboBoxStyle.DropDown;
                cbSqrft.DropDownStyle = cbDateSort.DropDownStyle = ComboBoxStyle.Simple;
                cbProjName.Text = "New Plan Reference";
                cbSqrft.Text = "";
                lblDateSort.Text = "Date: (mm/dd/yyyy) ";
                if (cbProjName.Items.Count != 0 && cbProjName.Items[0].ToString() == "Any")
                {
                    cbProjNum.Items.RemoveAt(0);
                    cbProjName.Items.RemoveAt(0);
                    cbClientName.Items.RemoveAt(0);
                    cbLocation.Items.RemoveAt(0);
                    cbPlan.Items.RemoveAt(0);
                    cbWidth.Items.RemoveAt(0);
                    cbDepth.Items.RemoveAt(0);
                    cbBeds.Items.RemoveAt(0);
                    cbBaths.Items.RemoveAt(0);
                }
            }
            else
            {
                cbProjNum.DropDownStyle =
                    cbProjName.DropDownStyle =
                    cbClientName.DropDownStyle =
                    cbLocation.DropDownStyle =
                    cbPlan.DropDownStyle =
                    cbWidth.DropDownStyle =
                    cbDepth.DropDownStyle =
                    cbBeds.DropDownStyle =
                    cbBaths.DropDownStyle =
                    cbSqrft.DropDownStyle =
                    cbDateSort.DropDownStyle = ComboBoxStyle.DropDownList;
                lblDateSort.Text = "Sort:";
                if (cbProjName.Items.Count == 0 || cbProjName.Items[0].ToString() != "Any")
                {
                    cbProjNum.Items.Insert(0, "Any");
                    cbProjName.Items.Insert(0, "Any");
                    cbClientName.Items.Insert(0, "Any");
                    cbLocation.Items.Insert(0, "Any");
                    cbPlan.Items.Insert(0, "Any");
                    cbWidth.Items.Insert(0, "Any");
                    cbDepth.Items.Insert(0, "Any");
                    cbBeds.Items.Insert(0, "Any");
                    cbBaths.Items.Insert(0, "Any");
                }
                cbProjNum.SelectedIndex =
                    cbProjName.SelectedIndex =
                    cbClientName.SelectedIndex =
                    cbLocation.SelectedIndex =
                    cbPlan.SelectedIndex =
                    cbWidth.SelectedIndex =
                    cbDepth.SelectedIndex =
                    cbBeds.SelectedIndex =
                    cbBaths.SelectedIndex =
                    cbSqrft.SelectedIndex = 
                    cbDateSort.SelectedIndex = 0;
            }

            dgList.ClearSelection();
            dgList.Rows[selectedRow].Selected = true;
        }
        /// <summary>
        /// Sets the values stored in the
        /// selected SitePlan to the combo
        /// boxes.
        /// </summary>
        private void SetValuesToComboBoxes()
        {
            if (dgList.SelectedRows.Count > 0 && _display.Count > 0)
            {
                SitePlan s = _display[dgList.SelectedRows[0].Index];
                cbProjNum.Text = s.ProjNumber;
                cbProjName.Text = s.ProjName;
                cbClientName.Text = s.ClientName;
                cbLocation.Text = s.Location;
                cbPlan.Text = s.Plan;
                cbWidth.Text = s.Width;
                cbDepth.Text = s.Depth;
                cbBeds.Text = s.Bedrooms;
                cbBaths.Text = s.Baths;
                cbSqrft.Text = s.Sqrft;
                cbDateSort.Text = s.Date;
                txtFile.Text = s.LinkPath;
            }
        }
        /// <summary>
        /// Sets the values stored in the
        /// combo boxes to the SitePlan
        /// selected.
        /// </summary>
        private SitePlan SetValuesToList()
        {
            if (dgList.SelectedRows.Count > 0 && _display.Count > 0)
            {
                SitePlan s = _display[dgList.SelectedRows[0].Index];
                s.ProjNumber = DataBase.ReadProjectNumber(cbProjNum.Text);
                s.ProjName = cbProjName.Text;
                s.ClientName = cbClientName.Text;
                s.Location = cbLocation.Text;
                s.Plan = cbPlan.Text;
                s.Width = cbWidth.Text;
                s.Depth = cbDepth.Text;
                s.Bedrooms = cbBeds.Text;
                s.Baths = cbBaths.Text;
                s.Sqrft = cbSqrft.Text;
                s.Date = cbDateSort.Text;
                s.LinkPath = txtFile.Text;
                return s;
            }
            return null;
        }
        private void ClearComboBoxes()
        {
            cbProjNum.Items.Clear();
            cbProjName.Items.Clear();
            cbClientName.Items.Clear();
            cbLocation.Items.Clear();
            cbPlan.Items.Clear();
            cbWidth.Items.Clear();
            cbDepth.Items.Clear();
            cbBeds.Items.Clear();
            cbBaths.Items.Clear();
        }
        private void UpdatePreview(string file)
        {
            string path = Directory.GetCurrentDirectory() + "/preview.html?filename=" + file;
            if (wbPreview.Url == null || wbPreview.Url != new Uri(path))
                wbPreview.Navigate(path);
        }
        /// <summary>
        /// Sets the widths of the controls
        /// and labels at the top of the 
        /// columns that do not naturally
        /// resize and move.
        /// </summary>
        private void UpdateColumnWidths()
        {
            //the placement of each control is determined by the end of the previous control
            //the width is determined by the column width
            const int spacing = 5;
            lblProjNum.Left = cbProjNum.Left = dgList.Left;
            cbProjNum.Width = dgList.Columns[0].Width - spacing;

            lblProjName.Left = cbProjName.Left = cbProjNum.Right + spacing;
            cbProjName.Width = dgList.Columns[1].Width - spacing;

            lblClient.Left = cbClientName.Left = cbProjName.Right + spacing;
            cbClientName.Width = dgList.Columns[2].Width - spacing;

            lblLocation.Left = cbLocation.Left = cbClientName.Right + spacing;
            cbLocation.Width = dgList.Columns[3].Width - spacing;

            lblPlan.Left = cbPlan.Left = cbLocation.Right + spacing;
            cbPlan.Width = dgList.Columns[4].Width - spacing;

            lblWidth.Left = cbWidth.Left = cbPlan.Right + spacing;
            cbWidth.Width = dgList.Columns[5].Width - spacing;

            lblDepth.Left = cbDepth.Left = cbWidth.Right + spacing;
            cbDepth.Width = dgList.Columns[6].Width - spacing;

            lblBeds.Left = cbBeds.Left = cbDepth.Right + spacing;
            cbBeds.Width = dgList.Columns[7].Width - spacing;

            lblBaths.Left = cbBaths.Left = cbBeds.Right + spacing;
            cbBaths.Width = dgList.Columns[8].Width - spacing;

            lblSqrft.Left = cbSqrft.Left = cbBaths.Right + spacing;
            cbSqrft.Width = dgList.Columns[9].Width - spacing;

            lblDateSort.Left = cbDateSort.Left = cbSqrft.Right + spacing;
            cbDateSort.Width = dgList.Columns[10].Width;
                
        }
        #endregion


        #region Self-Created Event Handlers
        /// <summary>
        /// Called whenever any of the combo boxes'
        /// SelectedIndexChanged event is raised.
        /// </summary>
        private void Search(object sender, EventArgs e)
        {
            //This try/catch block merely serves
            //as a backup net in case something
            //goes wrong, as this has proven to
            //be a problem area.
            try
            {
                //only preform code if not in "edit mode"
                if (mode == FormMode.Search)
                {
                    //creates an object to contain the search specifications
                    SitePlan query = new SitePlan(cbProjNum.Text, cbProjName.Text, cbClientName.Text, 
                        cbLocation.Text, cbPlan.Text, cbWidth.Text, cbDepth.Text, cbBeds.Text, cbBaths.Text, cbSqrft.Text, "", "");

                    //remembers which rows are selected
                    List<SitePlan> selected = new List<SitePlan>();
                    foreach (DataGridViewRow r in dgList.SelectedRows)
                        selected.Add((SitePlan)r.Tag);

                    //repopulates the list
                    _display.Clear();
                    foreach (SitePlan s in _sites)
                    {
                        if (query.IsEqualTo(s))
                            _display.Add(s);
                    }
                    //takes advantage of enums being stored as ints
                    SitePlan.Sort = (SortMethod)cbDateSort.SelectedIndex;
                    _display.Sort();

                    UpdateLists();

                    //returns selection to the rows
                    foreach (DataGridViewRow r in dgList.Rows)
                        r.Selected = selected.Contains((SitePlan)r.Tag);
                }
            }
            catch { }
        }
        /// <summary>
        /// This method switches the selected
        /// data base to search through and
        /// edit.
        /// </summary>
        private void ChangeDatabase(object sender, EventArgs e)
        {
            if (!saved)
            {
                DialogResult result = MessageBox.Show("Would you like to save before switching databases?\nAny unsaved data will be lost.", "Save Changes", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    //if the save is unsuccessful, 
                    //the new form will not appear
                    case DialogResult.Yes:  
                        if (SaveData())
                            break;
                        else
                            return;
                    case DialogResult.Cancel:
                        return;
                    default:
                        break;
                }
            }

            if (sender == tsFlat)
                DataBase.Type = DatabaseType.Flat;
            if (sender == tsSingle)
                    DataBase.Type = DatabaseType.SingleFamily;
            if (sender == tsTownhome)
                    DataBase.Type = DatabaseType.Townhome;
            if (sender == tsCarriage)
                    DataBase.Type = DatabaseType.Carriage;

            ClearComboBoxes();
            LoadData();
            dgList.ClearSelection();
            ChangeLayout(FormMode.Search);
            lblName.Text = "Database: " + DataBase.Type.ToString();
        }
        /// <summary>
        /// This method is neccisary to ensure
        /// that when the enter key is pressed
        /// it preforms the "save" or update
        /// feature. Paticularly the cbSqrft 
        /// control will handle the enter key
        /// in an undesired way.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && this.mode == FormMode.Edit)
            {
                btnUpdate_Click(this, new EventArgs());
                return false;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region Menu Item Event Handlers
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData())
                MessageBox.Show("Save Complete.");
        }
        private void saveExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveData())
            {
                MessageBox.Show("Save Complete.");
                this.Close();
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            this.Close();
        }
        private void tsCreateGroup_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.Cancel)
            {
                //gets all the files in the folder
                string[] files = Directory.GetFiles(fbd.SelectedPath);
                foreach (string f in files)
                {
                    //narrows the files to only pdfs
                    if (f.ToLower().Contains(".pdf"))
                    {
                        //checks to see if the pdf already exists
                        //in the current list
                        bool exists = false;
                        foreach (SitePlan s in _sites)
                            if (s.LinkPath == f)
                                exists = true;

                        if (!exists)
                        {
                            SitePlan addSite = new SitePlan();
                            addSite.LinkPath = f;
                            //removes the directory and the file extension
                            addSite.ProjName = f.Replace(fbd.SelectedPath + "\\", "").Replace(".pdf", "");
                            _sites.Add(addSite);
                            _display.Add(addSite);
                        }
                    }
                }

                UpdateLists();
                ChangeLayout(FormMode.Edit);
                SetValuesToComboBoxes();
                dgList.Rows[_display.Count - 1].Selected = true;
            }
        }
        #endregion

        #region Button Click Event Handlers
        private void btnOpen_Click(object sender, EventArgs e)
        {
            //calls 'Open' on all selected objects
            if (_display.Count > 0)
            {
                foreach (DataGridViewRow d in dgList.SelectedRows)
                {
                    try
                    {
                        _display[d.Index].Open();
                    }
                    catch (Win32Exception)
                    {
                        MessageBox.Show("File not found.");
                    }
                    catch
                    {
                        MessageBox.Show("Filename is blank.");
                    }
                }
            }
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            //Add a new site (the same new site)
            //to both the list of sites and the
            //list of displayed sites.
            SitePlan addSite = new SitePlan();
            _sites.Add(addSite);
            _display.Add(addSite);

            UpdateLists();
            ChangeLayout(FormMode.Edit);
            SetValuesToComboBoxes();
            //sets the selection to the last row which is
            //the newly created SitePlan.
            dgList.Rows[_display.Count - 1].Selected = true;
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgList.SelectedRows.Count > 0 && _display.Count > 0)
            {
                //Uses the first selected row if multiple rows
                //are selected.
                int index = dgList.SelectedRows[0].Index;
                ChangeLayout(FormMode.Edit);
                dgList.Rows[index].Selected = true;
                SetValuesToComboBoxes();
            }
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to remove this entry?", "", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
                return;

            try
            {
                int index = dgList.SelectedRows[0].Index;
                SitePlan removeSite = _display[index];
                _sites.Remove(removeSite);
                _display.Remove(removeSite);
                ClearComboBoxes();
                UpdateLists();
                ChangeLayout(FormMode.Search);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Cannot delete entry: \nIt is already empty.");
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SitePlan s = SetValuesToList();
            ClearComboBoxes();
            UpdateLists();
            ChangeLayout(FormMode.Search);

            if (s != null)
            {
                int i = _display.IndexOf(s);
                //highlights the item
                dgList.Rows[i].Selected = true;
                dgList.Focus();
                //set scrolling to the new item
                if (!dgList.Rows[i].Displayed)
                    dgList.FirstDisplayedScrollingRowIndex = i;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            SitePlan s = null;
            if (dgList.SelectedRows.Count > 0)
                s = _display[dgList.SelectedRows[0].Index];
            ClearComboBoxes();
            UpdateLists();
            ChangeLayout(FormMode.Search);

            if (s != null)
            {
                int i = _display.IndexOf(s);
                //highlights the item
                dgList.Rows[i].Selected = true;
                dgList.Focus();
                //set scrolling to the new item
                if (!dgList.Rows[i].Displayed)
                    dgList.FirstDisplayedScrollingRowIndex = i;
            }
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = fileDialog.ShowDialog();
            if (result != DialogResult.Cancel)
                txtFile.Text = fileDialog.FileName.Replace('\\', '/'); //this makes it a little safer for the html file
        }
        private void checkPreview_CheckedChanged(object sender, EventArgs e)
        {
            if (checkPreview.Checked && dgList.SelectedRows.Count > 0 && _display.Count > 0 && _display.Count == dgList.RowCount)
                UpdatePreview(_display[dgList.SelectedRows[0].Index].LinkPath);
            else
                UpdatePreview("");
        }
        #endregion

        #region DataGridView Event Handlers
        private void dgList_SelectionChanged(object sender, EventArgs e)
        {
            if (mode == FormMode.Edit)
                SetValuesToComboBoxes();

            if (dgList.SelectedRows.Count > 0 && _display.Count > 0 && _display.Count == dgList.RowCount)
            {
                string path = _display[dgList.SelectedRows[0].Index].LinkPath;
                if (checkPreview.Checked && !listPopulating)
                    UpdatePreview(path);
                txtFile.Text = path;
            }

            //sets focus once everything has
            //been properly updated
            if (mode == FormMode.Edit)
                cbProjNum.Focus();
        }
        private void dgList_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (mode == FormMode.Edit && dgList.SelectedRows.Count > 0 && dgList.SelectedRows[0].Index < _display.Count)
            {
                SetValuesToList();
                UpdateLists();
            }
        }
        private void dgList_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            UpdateColumnWidths();
        }
        private void dgList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEdit_Click(sender, e);
        }
        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!saved)
            {
                DialogResult result = MessageBox.Show("Would you like to save your changes before exiting?", "Save Changes", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    //if the save is unsuccessful, 
                    //the form will not close
                    e.Cancel = !SaveData();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            SaveLayout();
        }

    }
}
