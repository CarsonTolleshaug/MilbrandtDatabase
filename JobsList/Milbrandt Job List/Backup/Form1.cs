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
    public partial class Form1 : Form
    {
        List<Job> _display;
        List<Job> _jobs;
        SortOrder _sortDirection = SortOrder.Descending;

        public static char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public Form1()
        {
            InitializeComponent();
            LoadLayout();
            LoadData();
        }

        private void LoadData()
        {
            using (LoadingBox lb = new LoadingBox()) //works wonderfully without threading
            {
                lb.Show();
                lb.Update();

                try
                {
                    _jobs = Database.Read();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Cannot Read File: \n" + e.Message);
                    _jobs = new List<Job>();
                }
                _display = new List<Job>(_jobs);
                LastUpdatedLbl.Text = "Last Updated " + DateTime.Now.ToString();
                //System.Threading.Thread.Sleep(2000);
            }
        }

        private void LoadLayout()
        {
            string[] settings = Database.ReadLayout();
            try
            {
                //checks whether or not the window
                //should be maximized:
                if (bool.Parse(settings[0]))
                {
                    if (!bool.Parse(settings[5]))
                        this.Location = new Point(Screen.PrimaryScreen.Bounds.Width, 0);// puts it on the correct screen.
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    this.Location = new Point(int.Parse(settings[1]), int.Parse(settings[2]));
                    this.Width = int.Parse(settings[3]);
                    this.Height = int.Parse(settings[4]);
                    
                }
                for (int i = 0; i < dgList.ColumnCount && i + 6 < settings.Length; i++)
                    dgList.Columns[i].Width = int.Parse(settings[i + 6]);
            }
            catch
            {   }
        }
        private void SaveLayout()
        {
            string[] settings = new string[12];
            settings[0] = (this.WindowState == FormWindowState.Maximized).ToString();
            settings[1] = this.DesktopLocation.X.ToString();
            settings[2] = this.DesktopLocation.Y.ToString();
            settings[3] = this.Width.ToString();
            settings[4] = this.Height.ToString();
            settings[5] = Screen.FromControl(this).Primary.ToString();
            for (int i = 0; i < dgList.ColumnCount; i++)
                settings[i + 6] = dgList.Columns[i].Width.ToString();
            Database.WriteLayout(settings);
        }

        private void UpdateDisplay()
        {
            //Populating Data Grid View
            if (_display.Count != 0)
            {
                dgList.RowCount = _display.Count;
                _display.Sort(delegate(Job j1, Job j2)
                {
                    if (_sortDirection == SortOrder.Ascending)
                    {
                        Job temp = j1;
                        j1 = j2;
                        j2 = temp;
                    }

                    int compare = j2.JobNumber.Trim(alphabet).CompareTo(j1.JobNumber.Trim(alphabet));
                    if (compare == 0)
                        return j1.JobNumber.CompareTo(j2.JobNumber);
                    else
                        return compare;

                });
                for (int i = 0; i < _display.Count; i++)
                {
                    Job j = _display[i];
                    dgList.Rows[i].SetValues(j.JobNumber, j.ProjectTitle, j.ClientName, j.Assigned, j.Alias, j.Drawer);
                    dgList.Rows[i].Tag = j;

                    if (Directory.Exists(j.FileLocation))
                    {
                        if (j.FileLocation.StartsWith("H"))
                            dgList.Rows[i].Cells[0].Style.ForeColor = Color.Blue;
                        else
                        {
                            string newPath = j.FileLocation;
                            newPath = "H" + newPath.Remove(0, 1);
                            if (Directory.Exists(newPath))
                            {
                                dgList.Rows[i].Cells[0].Style.ForeColor = Color.Blue;
                                j.FileLocation = newPath;
                            }
                            else
                                dgList.Rows[i].Cells[0].Style.ForeColor = Color.DarkRed;
                        }
                    }
                    else
                    {
                        string newPath = j.FileLocation;
                        newPath = "H" + newPath.Remove(0, 1);
                        if (Directory.Exists(newPath))
                        {
                            dgList.Rows[i].Cells[0].Style.ForeColor = Color.Blue;
                            j.FileLocation = newPath;
                        }
                        else
                        {
                            newPath = "K" + newPath.Remove(0, 1);
                            if (Directory.Exists(newPath))
                            {
                                dgList.Rows[i].Cells[0].Style.ForeColor = Color.DarkRed;
                                j.FileLocation = newPath;
                            }
                            else
                                dgList.Rows[i].Cells[0].Style.ForeColor = Color.Black;
                        }
                    }

                    //string testpath1 = "H" + j.FileLocation.Remove(0, 1); //Deletes the first character (i.e. the drive letter)
                    //string testpath2 = "K" + j.FileLocation.Remove(0, 1);
                    //if (Directory.Exists(testpath1)) // Check the H drive
                    //{
                    //    //If it exists on the H drive, change the link and set the link to Dark Red
                    //    j.FileLocation = testpath1;
                    //    dgList.Rows[i].Cells[0].Style.ForeColor = Color.Blue;
                    //    Database.Write(_jobs);
                    //}
                    //else if (Directory.Exists(testpath2)) // Check the K drive
                    //{
                    //    //If it exists on the K drive, change the link and set the link to blue
                    //    j.FileLocation = testpath2;
                    //    dgList.Rows[i].Cells[0].Style.ForeColor = Color.DarkRed;
                    //    Database.Write(_jobs);
                    //}
                    //else if (Directory.Exists(j.FileLocation))
                    //{
                    //    if (j.FileLocation.StartsWith("H:")) //Just a precaution, should never happen
                    //        dgList.Rows[i].Cells[0].Style.ForeColor = Color.Blue;
                    //    else //If the link exists but is not on H or K make dark red
                    //        dgList.Rows[i].Cells[0].Style.ForeColor = Color.DarkRed;
                    //}
                    //else
                    //{
                    //    //Otherwise make the link black
                    //    dgList.Rows[i].Cells[0].Style.ForeColor = Color.Black;
                    //}
                }
            }
            else
            {
                dgList.RowCount = 1;
                dgList.Rows[0].SetValues("", "", "", "", "");
                dgList.Rows[0].Tag = null;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SearchBar_TextChanged(object sender, EventArgs e)
        {
            _display.Clear();
            foreach (Job j in _jobs)
            {
                if (j.ContainsQuery(SearchBar.Text))
                    _display.Add(j);
            }
            UpdateDisplay();
        }

        private List<string> ClientList()
        {
            List<string> clients = new List<string>();
            foreach (Job j in _jobs)
            {
                if (!clients.Contains(j.ClientName))
                    clients.Add(j.ClientName);
            }
            clients.Sort();
            return clients;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Job select = (Job)dgList.SelectedRows[0].Tag;
            if (select == null)
            {
                MessageBox.Show("Selection is empty.");
                return;
            }
                        
            // reselect the correct row
            bool success = false;
            foreach (DataGridViewRow r in dgList.Rows)
            {
                if (select.IsEqualTo(r.Tag as Job))
                {
                    r.Selected = success = true;
                    break;
                }
            }

            if (!success)
            {
                MessageBox.Show("Job has been removed or changed by another user.");
                return;
            }

            EditForm form = new EditForm(select, ClientList().ToArray());
            form.ShowDialog();
            while (EditForm.shouldRefresh)
            {
                LoadData();
                UpdateDisplay();
                EditForm.shouldRefresh = false;
                form = new EditForm(select, ClientList().ToArray());
                form.ShowDialog();
            }
            if (form.newJob != null)
            {
                if (!Database.Replace(select, form.newJob))
                {
                    MessageBox.Show("Someone was faster than you.\nAnother user has already edited or removed this job.");
                    LoadData();
                    UpdateDisplay();
                    return;
                }

                // However the above only changes the .dat file, we must still 
                // update the object currently in the list:
                select.JobNumber = form.newJob.JobNumber;
                select.ProjectTitle = form.newJob.ProjectTitle;
                select.ClientName = form.newJob.ClientName;
                select.FileLocation = form.newJob.FileLocation;
                select.Assigned = form.newJob.Assigned;
                select.Alias = form.newJob.Alias;
                select.Drawer = form.newJob.Drawer;

                LoadData();
                UpdateDisplay();
                SendEmail(form.newJob);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            LoadData();
            UpdateDisplay();

            EditForm form = new EditForm(null, ClientList().ToArray());
            form.ShowDialog();
            while (EditForm.shouldRefresh)
            {
                LoadData();
                UpdateDisplay();
                EditForm.shouldRefresh = false;
                form = new EditForm(null, ClientList().ToArray());
                form.ShowDialog();
            }
            if (form.newJob != null)
            {
                Job add = form.newJob;

                //save
                Database.Append(add);

                _jobs.Add(add);
                _display.Add(add);
                SearchBar.Text = "";
                UpdateDisplay();
                SendEmail(form.newJob);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            Job select = (Job)dgList.SelectedRows[0].Tag;
            if (select == null)
            {
                MessageBox.Show("Selection is empty.");
                return;
            }

            LoadData();
            UpdateDisplay();

            bool success = false;
            foreach (DataGridViewRow r in dgList.Rows)
            {
                if (select.IsEqualTo(r.Tag as Job))
                {
                    r.Selected = success = true;
                    break;
                }
            }

            if (!success)
            {
                MessageBox.Show("Job has been removed or changed by another user.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to remove this entry?", "", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
                return;

            try
            {
                int index = dgList.SelectedRows[0].Index;
                Job j = _display[index];
                _jobs.Remove(j);
                _display.Remove(j);
                Database.Write(_jobs);
                UpdateDisplay();
                SendEmail(j);
            }
            catch (ArgumentOutOfRangeException) //SelectedRows[0] does not exist
            {
                MessageBox.Show("Cannot delete entry: \nIt is already empty.");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveLayout();
        }

        private void dgList_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgList.SelectedRows.Count > 0 && e.ColumnIndex == 0)
            {
                if (_display.Count > 0 && dgList.SelectedRows[0].Cells[0].Style.ForeColor != Color.Black)
                {
                    try
                    {
                        (dgList.SelectedRows[0].Tag as Job).Open();
                    }
                    catch (Win32Exception)
                    {
                        MessageBox.Show("File not found.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
                btnEdit_Click(sender, e);
        }

        private void dgList_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            Form1_Resize(sender, e);
        }

        private void dgList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (_sortDirection == SortOrder.Ascending)
                    _sortDirection = SortOrder.Descending;
                else
                    _sortDirection = SortOrder.Ascending;
                UpdateDisplay();
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void SendEmail(Job j_changed)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

                //message prarams:
                message.To.Add("jgc@milbrandtarch.com");
                message.Subject = "Jobs list update";
                message.From = new System.Net.Mail.MailAddress("milbrandtjoblist@gmail.com");
                message.Body = "The following job was added/updated:\nJob Number: " +
                                j_changed.JobNumber + "\nProject Title: " + j_changed.ProjectTitle +
                                "\nClient Name: " + j_changed.ClientName + "\nFolder: " +
                                j_changed.FileLocation + "\n\nChanges made on " + DateTime.Now;

                //SMTP Set up:
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("milbrandtjoblist", "I8lulugm");
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot send email: " + ex.Message);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            dgList.Columns[5].Width = dgList.Right - (dgList.Columns[0].Width + dgList.Columns[1].Width + 
                                      dgList.Columns[2].Width + dgList.Columns[3].Width + dgList.Columns[4].Width) - 20;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            UpdateDisplay();
        }
    }
}
