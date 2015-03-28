namespace PlanReferenceDatabase
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsCreateGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDatabaseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsFlat = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSingle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsTownhome = new System.Windows.Forms.ToolStripMenuItem();
            this.tsCarriage = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wbPreview = new System.Windows.Forms.WebBrowser();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.ListArea = new System.Windows.Forms.Panel();
            this.cbDateSort = new System.Windows.Forms.ComboBox();
            this.lblDateSort = new System.Windows.Forms.Label();
            this.cbBaths = new System.Windows.Forms.ComboBox();
            this.cbBeds = new System.Windows.Forms.ComboBox();
            this.cbPlan = new System.Windows.Forms.ComboBox();
            this.cbClientName = new System.Windows.Forms.ComboBox();
            this.cbProjNum = new System.Windows.Forms.ComboBox();
            this.cbDepth = new System.Windows.Forms.ComboBox();
            this.cbWidth = new System.Windows.Forms.ComboBox();
            this.dgList = new System.Windows.Forms.DataGridView();
            this.ProjNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProjname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Plan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDepth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Beds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Baths = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSqrft = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbSqrft = new System.Windows.Forms.ComboBox();
            this.cbProjName = new System.Windows.Forms.ComboBox();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.lblSqrft = new System.Windows.Forms.Label();
            this.lblBaths = new System.Windows.Forms.Label();
            this.lblBeds = new System.Windows.Forms.Label();
            this.lblDepth = new System.Windows.Forms.Label();
            this.lblWidth = new System.Windows.Forms.Label();
            this.lblPlan = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblClient = new System.Windows.Forms.Label();
            this.lblProjName = new System.Windows.Forms.Label();
            this.lblProjNum = new System.Windows.Forms.Label();
            this.gbEdit = new System.Windows.Forms.GroupBox();
            this.lblToolTip = new System.Windows.Forms.Label();
            this.searchModeControls = new System.Windows.Forms.Panel();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.editModeControls = new System.Windows.Forms.Panel();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.lblPDF = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.checkPreview = new System.Windows.Forms.CheckBox();
            this.btnOpen1 = new System.Windows.Forms.Button();
            this.saveChangesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.singleFamilyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.townhomeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.carriageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblName = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.ListArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgList)).BeginInit();
            this.gbEdit.SuspendLayout();
            this.searchModeControls.SuspendLayout();
            this.editModeControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.changeDatabaseToolStripMenuItem1});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1158, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "Menu";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem1,
            this.saveExitToolStripMenuItem,
            this.tsCreateGroup,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Image = global::PlanReferenceDatabase.Properties.Resources.save;
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // saveExitToolStripMenuItem
            // 
            this.saveExitToolStripMenuItem.Name = "saveExitToolStripMenuItem";
            this.saveExitToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.saveExitToolStripMenuItem.Text = "Save and Exit";
            this.saveExitToolStripMenuItem.Click += new System.EventHandler(this.saveExitToolStripMenuItem_Click);
            // 
            // tsCreateGroup
            // 
            this.tsCreateGroup.Name = "tsCreateGroup";
            this.tsCreateGroup.Size = new System.Drawing.Size(216, 22);
            this.tsCreateGroup.Text = "Populate List from Folder...";
            this.tsCreateGroup.Click += new System.EventHandler(this.tsCreateGroup_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(213, 6);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // changeDatabaseToolStripMenuItem1
            // 
            this.changeDatabaseToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsFlat,
            this.tsSingle,
            this.tsTownhome,
            this.tsCarriage});
            this.changeDatabaseToolStripMenuItem1.Name = "changeDatabaseToolStripMenuItem1";
            this.changeDatabaseToolStripMenuItem1.Size = new System.Drawing.Size(111, 20);
            this.changeDatabaseToolStripMenuItem1.Text = "Change Database";
            // 
            // tsFlat
            // 
            this.tsFlat.Name = "tsFlat";
            this.tsFlat.Size = new System.Drawing.Size(144, 22);
            this.tsFlat.Text = "Flat";
            this.tsFlat.Click += new System.EventHandler(this.ChangeDatabase);
            // 
            // tsSingle
            // 
            this.tsSingle.Name = "tsSingle";
            this.tsSingle.Size = new System.Drawing.Size(144, 22);
            this.tsSingle.Text = "Single Family";
            this.tsSingle.Click += new System.EventHandler(this.ChangeDatabase);
            // 
            // tsTownhome
            // 
            this.tsTownhome.Name = "tsTownhome";
            this.tsTownhome.Size = new System.Drawing.Size(144, 22);
            this.tsTownhome.Text = "Townhome";
            this.tsTownhome.Click += new System.EventHandler(this.ChangeDatabase);
            // 
            // tsCarriage
            // 
            this.tsCarriage.Name = "tsCarriage";
            this.tsCarriage.Size = new System.Drawing.Size(144, 22);
            this.tsCarriage.Text = "Carriage";
            this.tsCarriage.Click += new System.EventHandler(this.ChangeDatabase);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(172, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // wbPreview
            // 
            this.wbPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.wbPreview.Location = new System.Drawing.Point(12, 28);
            this.wbPreview.Margin = new System.Windows.Forms.Padding(2);
            this.wbPreview.MinimumSize = new System.Drawing.Size(15, 16);
            this.wbPreview.Name = "wbPreview";
            this.wbPreview.ScrollBarsEnabled = false;
            this.wbPreview.Size = new System.Drawing.Size(408, 359);
            this.wbPreview.TabIndex = 0;
            this.wbPreview.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer.Location = new System.Drawing.Point(-2, 17);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer.Panel1.Controls.Add(this.ListArea);
            this.splitContainer.Panel1.Controls.Add(this.gbEdit);
            this.splitContainer.Panel1.Controls.Add(this.btnOpen);
            this.splitContainer.Panel1MinSize = 350;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer.Panel2.Controls.Add(this.checkPreview);
            this.splitContainer.Panel2.Controls.Add(this.btnOpen1);
            this.splitContainer.Panel2.Controls.Add(this.wbPreview);
            this.splitContainer.Panel2MinSize = 100;
            this.splitContainer.Size = new System.Drawing.Size(1162, 448);
            this.splitContainer.SplitterDistance = 719;
            this.splitContainer.SplitterWidth = 2;
            this.splitContainer.TabIndex = 25;
            // 
            // ListArea
            // 
            this.ListArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ListArea.AutoScroll = true;
            this.ListArea.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ListArea.Controls.Add(this.cbDateSort);
            this.ListArea.Controls.Add(this.lblDateSort);
            this.ListArea.Controls.Add(this.cbBaths);
            this.ListArea.Controls.Add(this.cbBeds);
            this.ListArea.Controls.Add(this.cbPlan);
            this.ListArea.Controls.Add(this.cbClientName);
            this.ListArea.Controls.Add(this.cbProjNum);
            this.ListArea.Controls.Add(this.cbDepth);
            this.ListArea.Controls.Add(this.cbWidth);
            this.ListArea.Controls.Add(this.dgList);
            this.ListArea.Controls.Add(this.cbSqrft);
            this.ListArea.Controls.Add(this.cbProjName);
            this.ListArea.Controls.Add(this.cbLocation);
            this.ListArea.Controls.Add(this.lblSqrft);
            this.ListArea.Controls.Add(this.lblBaths);
            this.ListArea.Controls.Add(this.lblBeds);
            this.ListArea.Controls.Add(this.lblDepth);
            this.ListArea.Controls.Add(this.lblWidth);
            this.ListArea.Controls.Add(this.lblPlan);
            this.ListArea.Controls.Add(this.lblLocation);
            this.ListArea.Controls.Add(this.lblClient);
            this.ListArea.Controls.Add(this.lblProjName);
            this.ListArea.Controls.Add(this.lblProjNum);
            this.ListArea.Location = new System.Drawing.Point(10, 11);
            this.ListArea.Margin = new System.Windows.Forms.Padding(2);
            this.ListArea.Name = "ListArea";
            this.ListArea.Size = new System.Drawing.Size(703, 334);
            this.ListArea.TabIndex = 50;
            // 
            // cbDateSort
            // 
            this.cbDateSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDateSort.FormattingEnabled = true;
            this.cbDateSort.Items.AddRange(new object[] {
            "Default",
            "Date: Accending",
            "Date: Decending"});
            this.cbDateSort.Location = new System.Drawing.Point(571, 41);
            this.cbDateSort.Name = "cbDateSort";
            this.cbDateSort.Size = new System.Drawing.Size(56, 21);
            this.cbDateSort.TabIndex = 70;
            this.cbDateSort.SelectedIndexChanged += new System.EventHandler(this.Search);
            // 
            // lblDateSort
            // 
            this.lblDateSort.Location = new System.Drawing.Point(569, 24);
            this.lblDateSort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDateSort.Name = "lblDateSort";
            this.lblDateSort.Size = new System.Drawing.Size(119, 17);
            this.lblDateSort.TabIndex = 71;
            this.lblDateSort.Text = "Sort:";
            // 
            // cbBaths
            // 
            this.cbBaths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBaths.FormattingEnabled = true;
            this.cbBaths.Location = new System.Drawing.Point(452, 42);
            this.cbBaths.Name = "cbBaths";
            this.cbBaths.Size = new System.Drawing.Size(50, 21);
            this.cbBaths.TabIndex = 8;
            this.cbBaths.SelectedIndexChanged += new System.EventHandler(this.Search);
            // 
            // cbBeds
            // 
            this.cbBeds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBeds.FormattingEnabled = true;
            this.cbBeds.Location = new System.Drawing.Point(398, 42);
            this.cbBeds.Name = "cbBeds";
            this.cbBeds.Size = new System.Drawing.Size(50, 21);
            this.cbBeds.TabIndex = 7;
            this.cbBeds.SelectedIndexChanged += new System.EventHandler(this.Search);
            // 
            // cbPlan
            // 
            this.cbPlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlan.FormattingEnabled = true;
            this.cbPlan.Location = new System.Drawing.Point(230, 41);
            this.cbPlan.Name = "cbPlan";
            this.cbPlan.Size = new System.Drawing.Size(50, 21);
            this.cbPlan.TabIndex = 4;
            this.cbPlan.SelectedIndexChanged += new System.EventHandler(this.Search);
            // 
            // cbClientName
            // 
            this.cbClientName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClientName.FormattingEnabled = true;
            this.cbClientName.Location = new System.Drawing.Point(117, 41);
            this.cbClientName.Name = "cbClientName";
            this.cbClientName.Size = new System.Drawing.Size(50, 21);
            this.cbClientName.TabIndex = 2;
            this.cbClientName.SelectedIndexChanged += new System.EventHandler(this.Search);
            // 
            // cbProjNum
            // 
            this.cbProjNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProjNum.FormattingEnabled = true;
            this.cbProjNum.Location = new System.Drawing.Point(4, 41);
            this.cbProjNum.Name = "cbProjNum";
            this.cbProjNum.Size = new System.Drawing.Size(50, 21);
            this.cbProjNum.TabIndex = 0;
            this.cbProjNum.SelectedIndexChanged += new System.EventHandler(this.Search);
            // 
            // cbDepth
            // 
            this.cbDepth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDepth.FormattingEnabled = true;
            this.cbDepth.Location = new System.Drawing.Point(341, 41);
            this.cbDepth.Name = "cbDepth";
            this.cbDepth.Size = new System.Drawing.Size(50, 21);
            this.cbDepth.TabIndex = 6;
            this.cbDepth.SelectedIndexChanged += new System.EventHandler(this.Search);
            // 
            // cbWidth
            // 
            this.cbWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWidth.FormattingEnabled = true;
            this.cbWidth.Location = new System.Drawing.Point(286, 41);
            this.cbWidth.Name = "cbWidth";
            this.cbWidth.Size = new System.Drawing.Size(50, 21);
            this.cbWidth.TabIndex = 5;
            this.cbWidth.SelectedIndexChanged += new System.EventHandler(this.Search);
            // 
            // dgList
            // 
            this.dgList.AllowUserToResizeRows = false;
            this.dgList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgList.ColumnHeadersVisible = false;
            this.dgList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProjNum,
            this.colProjname,
            this.ClientName,
            this.colState,
            this.Plan,
            this.colWidth,
            this.colDepth,
            this.Beds,
            this.Baths,
            this.colSqrft,
            this.Date});
            this.dgList.Location = new System.Drawing.Point(0, 67);
            this.dgList.Margin = new System.Windows.Forms.Padding(2);
            this.dgList.Name = "dgList";
            this.dgList.RowHeadersVisible = false;
            this.dgList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgList.Size = new System.Drawing.Size(694, 267);
            this.dgList.TabIndex = 56;
            this.dgList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgList_CellDoubleClick);
            this.dgList.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgList_RowValidating);
            this.dgList.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgList_ColumnWidthChanged);
            this.dgList.SelectionChanged += new System.EventHandler(this.dgList_SelectionChanged);
            // 
            // ProjNum
            // 
            this.ProjNum.HeaderText = "Project Number";
            this.ProjNum.MinimumWidth = 50;
            this.ProjNum.Name = "ProjNum";
            this.ProjNum.ReadOnly = true;
            this.ProjNum.Width = 75;
            // 
            // colProjname
            // 
            this.colProjname.HeaderText = "Project Name";
            this.colProjname.MinimumWidth = 50;
            this.colProjname.Name = "colProjname";
            this.colProjname.ReadOnly = true;
            this.colProjname.Width = 75;
            // 
            // ClientName
            // 
            this.ClientName.HeaderText = "Client Name";
            this.ClientName.MinimumWidth = 50;
            this.ClientName.Name = "ClientName";
            this.ClientName.ReadOnly = true;
            this.ClientName.Width = 75;
            // 
            // colState
            // 
            this.colState.HeaderText = "Location";
            this.colState.MinimumWidth = 50;
            this.colState.Name = "colState";
            this.colState.ReadOnly = true;
            this.colState.Width = 75;
            // 
            // Plan
            // 
            this.Plan.HeaderText = "Plan";
            this.Plan.MinimumWidth = 50;
            this.Plan.Name = "Plan";
            this.Plan.ReadOnly = true;
            this.Plan.Width = 75;
            // 
            // colWidth
            // 
            this.colWidth.HeaderText = "Width";
            this.colWidth.MinimumWidth = 50;
            this.colWidth.Name = "colWidth";
            this.colWidth.ReadOnly = true;
            this.colWidth.Width = 75;
            // 
            // colDepth
            // 
            this.colDepth.HeaderText = "Depth";
            this.colDepth.MinimumWidth = 50;
            this.colDepth.Name = "colDepth";
            this.colDepth.ReadOnly = true;
            this.colDepth.Width = 75;
            // 
            // Beds
            // 
            this.Beds.HeaderText = "Bedrooms";
            this.Beds.MinimumWidth = 50;
            this.Beds.Name = "Beds";
            this.Beds.ReadOnly = true;
            this.Beds.Width = 75;
            // 
            // Baths
            // 
            this.Baths.HeaderText = "Bathrooms";
            this.Baths.MinimumWidth = 50;
            this.Baths.Name = "Baths";
            this.Baths.ReadOnly = true;
            this.Baths.Width = 75;
            // 
            // colSqrft
            // 
            this.colSqrft.HeaderText = "Square Feet";
            this.colSqrft.MinimumWidth = 70;
            this.colSqrft.Name = "colSqrft";
            this.colSqrft.ReadOnly = true;
            this.colSqrft.Width = 75;
            // 
            // Date
            // 
            this.Date.HeaderText = "Date";
            this.Date.MinimumWidth = 75;
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            // 
            // cbSqrft
            // 
            this.cbSqrft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSqrft.FormattingEnabled = true;
            this.cbSqrft.Items.AddRange(new object[] {
            "Any",
            "501 - 750",
            "751 - 1000",
            "1001 - 1250",
            "1251 - 1500",
            "1501 - 1750",
            "1751 - 2000",
            "2001 - 2250",
            "2251 - 2500",
            "2501 - 2750",
            "2751 - 3000",
            "3001 - 3250",
            "3251 - 3500",
            "3501 - 3750",
            "3751 - 4000"});
            this.cbSqrft.Location = new System.Drawing.Point(508, 41);
            this.cbSqrft.Name = "cbSqrft";
            this.cbSqrft.Size = new System.Drawing.Size(56, 21);
            this.cbSqrft.TabIndex = 9;
            this.cbSqrft.SelectedIndexChanged += new System.EventHandler(this.Search);
            // 
            // cbProjName
            // 
            this.cbProjName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProjName.FormattingEnabled = true;
            this.cbProjName.Location = new System.Drawing.Point(61, 41);
            this.cbProjName.Name = "cbProjName";
            this.cbProjName.Size = new System.Drawing.Size(50, 21);
            this.cbProjName.TabIndex = 1;
            this.cbProjName.SelectedIndexChanged += new System.EventHandler(this.Search);
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.FormattingEnabled = true;
            this.cbLocation.Location = new System.Drawing.Point(172, 41);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(51, 21);
            this.cbLocation.TabIndex = 3;
            this.cbLocation.SelectedIndexChanged += new System.EventHandler(this.Search);
            // 
            // lblSqrft
            // 
            this.lblSqrft.Location = new System.Drawing.Point(506, 10);
            this.lblSqrft.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSqrft.Name = "lblSqrft";
            this.lblSqrft.Size = new System.Drawing.Size(58, 31);
            this.lblSqrft.TabIndex = 55;
            this.lblSqrft.Text = "Square Feet:";
            // 
            // lblBaths
            // 
            this.lblBaths.Location = new System.Drawing.Point(450, 24);
            this.lblBaths.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBaths.Name = "lblBaths";
            this.lblBaths.Size = new System.Drawing.Size(37, 16);
            this.lblBaths.TabIndex = 69;
            this.lblBaths.Text = "Baths:";
            // 
            // lblBeds
            // 
            this.lblBeds.Location = new System.Drawing.Point(395, 24);
            this.lblBeds.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBeds.Name = "lblBeds";
            this.lblBeds.Size = new System.Drawing.Size(34, 16);
            this.lblBeds.TabIndex = 67;
            this.lblBeds.Text = "Beds:";
            // 
            // lblDepth
            // 
            this.lblDepth.AutoSize = true;
            this.lblDepth.Location = new System.Drawing.Point(341, 24);
            this.lblDepth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDepth.Name = "lblDepth";
            this.lblDepth.Size = new System.Drawing.Size(39, 13);
            this.lblDepth.TabIndex = 60;
            this.lblDepth.Text = "Depth:";
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(286, 24);
            this.lblWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(38, 13);
            this.lblWidth.TabIndex = 58;
            this.lblWidth.Text = "Width:";
            // 
            // lblPlan
            // 
            this.lblPlan.Location = new System.Drawing.Point(226, 10);
            this.lblPlan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPlan.Name = "lblPlan";
            this.lblPlan.Size = new System.Drawing.Size(68, 31);
            this.lblPlan.TabIndex = 65;
            this.lblPlan.Text = "Unit/House Plan:";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(172, 24);
            this.lblLocation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 13);
            this.lblLocation.TabIndex = 52;
            this.lblLocation.Text = "Location:";
            // 
            // lblClient
            // 
            this.lblClient.Location = new System.Drawing.Point(116, 10);
            this.lblClient.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(38, 34);
            this.lblClient.TabIndex = 63;
            this.lblClient.Text = "Client Name:";
            // 
            // lblProjName
            // 
            this.lblProjName.Location = new System.Drawing.Point(60, 10);
            this.lblProjName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProjName.Name = "lblProjName";
            this.lblProjName.Size = new System.Drawing.Size(40, 34);
            this.lblProjName.TabIndex = 51;
            this.lblProjName.Text = "Project Name:";
            // 
            // lblProjNum
            // 
            this.lblProjNum.Location = new System.Drawing.Point(4, 10);
            this.lblProjNum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProjNum.Name = "lblProjNum";
            this.lblProjNum.Size = new System.Drawing.Size(47, 34);
            this.lblProjNum.TabIndex = 61;
            this.lblProjNum.Text = "Project Number:";
            // 
            // gbEdit
            // 
            this.gbEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbEdit.Controls.Add(this.lblToolTip);
            this.gbEdit.Controls.Add(this.searchModeControls);
            this.gbEdit.Controls.Add(this.editModeControls);
            this.gbEdit.Location = new System.Drawing.Point(8, 349);
            this.gbEdit.Margin = new System.Windows.Forms.Padding(2);
            this.gbEdit.Name = "gbEdit";
            this.gbEdit.Padding = new System.Windows.Forms.Padding(2);
            this.gbEdit.Size = new System.Drawing.Size(706, 87);
            this.gbEdit.TabIndex = 5;
            this.gbEdit.TabStop = false;
            this.gbEdit.Text = "Editing Controls:";
            // 
            // lblToolTip
            // 
            this.lblToolTip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblToolTip.AutoSize = true;
            this.lblToolTip.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToolTip.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblToolTip.Location = new System.Drawing.Point(5, 70);
            this.lblToolTip.Name = "lblToolTip";
            this.lblToolTip.Size = new System.Drawing.Size(224, 13);
            this.lblToolTip.TabIndex = 26;
            this.lblToolTip.Text = "*Remove Row is only available in \"edit mode\".";
            // 
            // searchModeControls
            // 
            this.searchModeControls.Controls.Add(this.btnEdit);
            this.searchModeControls.Controls.Add(this.btnCreate);
            this.searchModeControls.Location = new System.Drawing.Point(4, 18);
            this.searchModeControls.Name = "searchModeControls";
            this.searchModeControls.Size = new System.Drawing.Size(211, 23);
            this.searchModeControls.TabIndex = 24;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(115, -1);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(2);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(96, 23);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Edit Row";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(0, -1);
            this.btnCreate.Margin = new System.Windows.Forms.Padding(2);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(94, 23);
            this.btnCreate.TabIndex = 4;
            this.btnCreate.Text = "Add Row";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // editModeControls
            // 
            this.editModeControls.Controls.Add(this.txtFile);
            this.editModeControls.Controls.Add(this.lblPDF);
            this.editModeControls.Controls.Add(this.btnCancel);
            this.editModeControls.Controls.Add(this.btnBrowse);
            this.editModeControls.Controls.Add(this.btnUpdate);
            this.editModeControls.Controls.Add(this.btnRemove);
            this.editModeControls.Location = new System.Drawing.Point(2, 17);
            this.editModeControls.Name = "editModeControls";
            this.editModeControls.Size = new System.Drawing.Size(690, 52);
            this.editModeControls.TabIndex = 25;
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(236, 30);
            this.txtFile.Margin = new System.Windows.Forms.Padding(2);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(382, 20);
            this.txtFile.TabIndex = 0;
            // 
            // lblPDF
            // 
            this.lblPDF.AutoSize = true;
            this.lblPDF.Location = new System.Drawing.Point(233, 5);
            this.lblPDF.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPDF.Name = "lblPDF";
            this.lblPDF.Size = new System.Drawing.Size(76, 13);
            this.lblPDF.TabIndex = 23;
            this.lblPDF.Text = "PDF Filename:";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(164, 27);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(49, 24);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(622, 27);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(66, 24);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(117, 27);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(2);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(43, 24);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Save";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(2, 27);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(94, 24);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "Remove Row";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.Location = new System.Drawing.Point(773, 364);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(127, 23);
            this.btnOpen.TabIndex = 37;
            this.btnOpen.Text = "Open PDF";
            this.btnOpen.UseVisualStyleBackColor = true;
            // 
            // checkPreview
            // 
            this.checkPreview.AutoSize = true;
            this.checkPreview.Location = new System.Drawing.Point(12, 11);
            this.checkPreview.Name = "checkPreview";
            this.checkPreview.Size = new System.Drawing.Size(97, 17);
            this.checkPreview.TabIndex = 25;
            this.checkPreview.Text = "Show Preview:";
            this.checkPreview.UseVisualStyleBackColor = true;
            this.checkPreview.CheckedChanged += new System.EventHandler(this.checkPreview_CheckedChanged);
            // 
            // btnOpen1
            // 
            this.btnOpen1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOpen1.Location = new System.Drawing.Point(169, 404);
            this.btnOpen1.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpen1.Name = "btnOpen1";
            this.btnOpen1.Size = new System.Drawing.Size(94, 24);
            this.btnOpen1.TabIndex = 1;
            this.btnOpen1.Text = "Open PDF";
            this.btnOpen1.UseVisualStyleBackColor = true;
            this.btnOpen1.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // saveChangesToolStripMenuItem
            // 
            this.saveChangesToolStripMenuItem.Name = "saveChangesToolStripMenuItem";
            this.saveChangesToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.saveChangesToolStripMenuItem.Text = "Save Changes...";
            this.saveChangesToolStripMenuItem.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::PlanReferenceDatabase.Properties.Resources.save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // changeDatabaseToolStripMenuItem
            // 
            this.changeDatabaseToolStripMenuItem.Name = "changeDatabaseToolStripMenuItem";
            this.changeDatabaseToolStripMenuItem.Size = new System.Drawing.Size(111, 20);
            this.changeDatabaseToolStripMenuItem.Text = "Change Database";
            // 
            // flatToolStripMenuItem
            // 
            this.flatToolStripMenuItem.Name = "flatToolStripMenuItem";
            this.flatToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.flatToolStripMenuItem.Text = "Flat";
            // 
            // singleFamilyToolStripMenuItem
            // 
            this.singleFamilyToolStripMenuItem.Name = "singleFamilyToolStripMenuItem";
            this.singleFamilyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.singleFamilyToolStripMenuItem.Text = "Single Family";
            // 
            // townhomeToolStripMenuItem
            // 
            this.townhomeToolStripMenuItem.Name = "townhomeToolStripMenuItem";
            this.townhomeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.townhomeToolStripMenuItem.Text = "Townhome";
            // 
            // carriageToolStripMenuItem
            // 
            this.carriageToolStripMenuItem.Name = "carriageToolStripMenuItem";
            this.carriageToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.carriageToolStripMenuItem.Text = "Carriage";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(249, 6);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(95, 13);
            this.lblName.TabIndex = 26;
            this.lblName.Text = "Database name";
            // 
            // Form1
            // 
            this.AcceptButton = this.btnUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1158, 464);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.splitContainer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(581, 237);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Floor Plan Database";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            this.splitContainer.ResumeLayout(false);
            this.ListArea.ResumeLayout(false);
            this.ListArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgList)).EndInit();
            this.gbEdit.ResumeLayout(false);
            this.gbEdit.PerformLayout();
            this.searchModeControls.ResumeLayout(false);
            this.editModeControls.ResumeLayout(false);
            this.editModeControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveChangesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.WebBrowser wbPreview;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox gbEdit;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblPDF;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnOpen1;
        private System.Windows.Forms.Panel ListArea;
        private System.Windows.Forms.ComboBox cbBaths;
        private System.Windows.Forms.Label lblBaths;
        private System.Windows.Forms.ComboBox cbBeds;
        private System.Windows.Forms.Label lblBeds;
        private System.Windows.Forms.ComboBox cbPlan;
        private System.Windows.Forms.Label lblPlan;
        private System.Windows.Forms.ComboBox cbClientName;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.ComboBox cbProjNum;
        private System.Windows.Forms.Label lblProjNum;
        private System.Windows.Forms.Label lblDepth;
        private System.Windows.Forms.ComboBox cbDepth;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.ComboBox cbWidth;
        private System.Windows.Forms.DataGridView dgList;
        private System.Windows.Forms.Label lblSqrft;
        private System.Windows.Forms.ComboBox cbSqrft;
        private System.Windows.Forms.ComboBox cbProjName;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblProjName;
        private System.Windows.Forms.ComboBox cbLocation;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem singleFamilyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem townhomeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem carriageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem changeDatabaseToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsFlat;
        private System.Windows.Forms.ToolStripMenuItem tsSingle;
        private System.Windows.Forms.ToolStripMenuItem tsTownhome;
        private System.Windows.Forms.ToolStripMenuItem tsCarriage;
        private System.Windows.Forms.Panel searchModeControls;
        private System.Windows.Forms.Panel editModeControls;
        private System.Windows.Forms.ToolStripMenuItem tsCreateGroup;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ToolStripMenuItem saveExitToolStripMenuItem;
        private System.Windows.Forms.Label lblToolTip;
        private System.Windows.Forms.CheckBox checkPreview;
        private System.Windows.Forms.ComboBox cbDateSort;
        private System.Windows.Forms.Label lblDateSort;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjname;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.DataGridViewTextBoxColumn Plan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDepth;
        private System.Windows.Forms.DataGridViewTextBoxColumn Beds;
        private System.Windows.Forms.DataGridViewTextBoxColumn Baths;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSqrft;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
    }
}

