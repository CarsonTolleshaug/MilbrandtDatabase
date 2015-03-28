namespace PlanReferenceDatabase
{
    partial class StartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.Exit = new System.Windows.Forms.Button();
            this.btnFlat = new System.Windows.Forms.Button();
            this.btnTownhome = new System.Windows.Forms.Button();
            this.btnSingleFamily = new System.Windows.Forms.Button();
            this.btnCarriage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Exit
            // 
            this.Exit.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.Exit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Exit.Location = new System.Drawing.Point(663, 5);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(19, 21);
            this.Exit.TabIndex = 0;
            this.Exit.Text = "X";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // btnFlat
            // 
            this.btnFlat.BackColor = System.Drawing.Color.Gainsboro;
            this.btnFlat.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFlat.BackgroundImage")));
            this.btnFlat.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnFlat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFlat.Font = new System.Drawing.Font("Euro Sign", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFlat.Location = new System.Drawing.Point(178, 95);
            this.btnFlat.Name = "btnFlat";
            this.btnFlat.Size = new System.Drawing.Size(153, 30);
            this.btnFlat.TabIndex = 1;
            this.btnFlat.Text = "Flat";
            this.btnFlat.UseVisualStyleBackColor = false;
            this.btnFlat.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btnFlat.Click += new System.EventHandler(this.btnFlat_Click);
            this.btnFlat.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            // 
            // btnTownhome
            // 
            this.btnTownhome.BackColor = System.Drawing.Color.Gainsboro;
            this.btnTownhome.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTownhome.BackgroundImage")));
            this.btnTownhome.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnTownhome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTownhome.Font = new System.Drawing.Font("Euro Sign", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTownhome.Location = new System.Drawing.Point(376, 95);
            this.btnTownhome.Name = "btnTownhome";
            this.btnTownhome.Size = new System.Drawing.Size(153, 30);
            this.btnTownhome.TabIndex = 2;
            this.btnTownhome.Text = "Townhome";
            this.btnTownhome.UseVisualStyleBackColor = false;
            this.btnTownhome.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btnTownhome.Click += new System.EventHandler(this.btnTownhome_Click);
            this.btnTownhome.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            // 
            // btnSingleFamily
            // 
            this.btnSingleFamily.BackColor = System.Drawing.Color.Gainsboro;
            this.btnSingleFamily.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSingleFamily.BackgroundImage")));
            this.btnSingleFamily.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnSingleFamily.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSingleFamily.Font = new System.Drawing.Font("Euro Sign", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSingleFamily.Location = new System.Drawing.Point(178, 131);
            this.btnSingleFamily.Name = "btnSingleFamily";
            this.btnSingleFamily.Size = new System.Drawing.Size(153, 30);
            this.btnSingleFamily.TabIndex = 2;
            this.btnSingleFamily.Text = "Single Family";
            this.btnSingleFamily.UseVisualStyleBackColor = false;
            this.btnSingleFamily.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btnSingleFamily.Click += new System.EventHandler(this.btnSingleFamily_Click);
            this.btnSingleFamily.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            // 
            // btnCarriage
            // 
            this.btnCarriage.BackColor = System.Drawing.Color.Gainsboro;
            this.btnCarriage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCarriage.BackgroundImage")));
            this.btnCarriage.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnCarriage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCarriage.Font = new System.Drawing.Font("Euro Sign", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCarriage.Location = new System.Drawing.Point(376, 131);
            this.btnCarriage.Name = "btnCarriage";
            this.btnCarriage.Size = new System.Drawing.Size(153, 30);
            this.btnCarriage.TabIndex = 2;
            this.btnCarriage.Text = "Carriage";
            this.btnCarriage.UseVisualStyleBackColor = false;
            this.btnCarriage.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btnCarriage.Click += new System.EventHandler(this.btnCarriage_Click);
            this.btnCarriage.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PlanReferenceDatabase.Properties.Resources.StartForm1;
            this.ClientSize = new System.Drawing.Size(687, 225);
            this.Controls.Add(this.btnSingleFamily);
            this.Controls.Add(this.btnCarriage);
            this.Controls.Add(this.btnTownhome);
            this.Controls.Add(this.btnFlat);
            this.Controls.Add(this.Exit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StartForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.Button btnFlat;
        private System.Windows.Forms.Button btnTownhome;
        private System.Windows.Forms.Button btnSingleFamily;
        private System.Windows.Forms.Button btnCarriage;
    }
}