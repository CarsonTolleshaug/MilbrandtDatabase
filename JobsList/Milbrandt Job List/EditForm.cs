using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HyperlinkJobsList
{
    public partial class EditForm : Form
    {
        public Job newJob;
        public static bool shouldRefresh = false;
        private const int space = 20;

        public EditForm(Job j_edit, string[] clients)
        {
            InitializeComponent();
            newJob = null;
            loadEmployees(Database.empFile);

            if (j_edit != null)
            {
                txtJobNum.Text = j_edit.JobNumber;
                txtProject.Text = j_edit.ProjectTitle;
                txtFileName.Text = j_edit.FileLocation;
                checkEmployees(j_edit.Assigned);
                txtAlias.Text = j_edit.Alias;
                txtDrawer.Text = j_edit.Drawer;
            }

            if (clients.Length != 0)
            {
                txtClient.Items.AddRange(clients);
                if (j_edit != null)
                    txtClient.SelectedIndex = txtClient.Items.IndexOf(j_edit.ClientName);
                else
                    txtClient.SelectedIndex = 0;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtFileName.Text = fbd.SelectedPath;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            newJob = null;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtJobNum.Text == "") {
                MessageBox.Show("Job # is blank.");
                return;
            }
            if (txtProject.Text == "") {
                MessageBox.Show("Project Title is blank.");
                return;
            }
            if (txtClient.Text == "") {
                MessageBox.Show("Client Name is blank.");
                return;
            }
            
            // Checkboxes:
            List<string> str = new List<string>();
            foreach (Control c in pnlAsn.Controls)
            {
                CheckBox cb = (CheckBox)c;
                if (cb.Checked)
                    str.Add(cb.Text);
            }
            string asn = string.Join(", ", str.ToArray());
            
            newJob = new Job(txtJobNum.Text, txtProject.Text, txtClient.Text, txtFileName.Text, asn, txtAlias.Text, txtDrawer.Text);
            this.Close();
        }

        private void txtJobNum_Validated(object sender, EventArgs e)
        {
            string testpath;
            if (txtFileName.Text == "")
            {        
                if (txtJobNum.Text.Contains('-'))
                    txtJobNum.Text = txtJobNum.Text.Remove(txtJobNum.Text.IndexOf('-'), 1);
                testpath = "H:\\" + txtJobNum.Text.Trim(Form1.alphabet).Trim(Form1.alphabet.ToString().ToLower().ToCharArray());
                if (!Directory.Exists(testpath))
                {
                    testpath = "K:\\" + txtJobNum.Text.Trim(Form1.alphabet).Trim(Form1.alphabet.ToString().ToLower().ToCharArray());
                    if (!Directory.Exists(testpath))
                    {
                        testpath = "H:\\" + txtJobNum.Text.Trim(Form1.alphabet).Trim(Form1.alphabet.ToString().ToLower().ToCharArray());
                    }
                }
                txtFileName.Text = testpath;
            }
        }

        private void txtFileName_Validated(object sender, EventArgs e)
        {
            if (txtJobNum.Text == "")
            {
                try
                {
                    txtJobNum.Text = txtFileName.Text.Split("\\".ToCharArray(), 2)[1];
                }
                catch { }
            }
        }

        private void loadEmployees(string empFileName)
        {
            if (File.Exists(empFileName))
            {
                StreamReader empFile = new StreamReader(empFileName);
                while (!empFile.EndOfStream)
                {
                    CheckBox cb = new CheckBox();
                    cb.Text = empFile.ReadLine();
                    pnlAsn.Controls.Add(cb);
                    cb.Location = new Point(space, pnlAsn.Controls.Count * space);
                }
                empFile.Close();

                this.Height += (pnlAsn.Controls.Count -1) * space;
            }
        }

        private void checkEmployees(string assignedEmps)
        {
            string[] strAsn = assignedEmps.Split(", ".ToCharArray());
            foreach (Control c in pnlAsn.Controls)
            {
                CheckBox cb = (CheckBox)c;
                cb.Checked = false;

                foreach (string a in strAsn)
                    if (cb.Text == a)
                        cb.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            shouldRefresh = true;
            Close();
        }
    }
}
