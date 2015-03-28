using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PlanReferenceDatabase
{
    public partial class StartForm : Form
    {
        public static bool ShouldRunMain = true;

        public StartForm()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
            ShouldRunMain = false;
        }

        private void btnFlat_Click(object sender, EventArgs e)
        {
            DataBase.Type = DatabaseType.Flat;
            this.Close();
        }

        private void btnSingleFamily_Click(object sender, EventArgs e)
        {
            DataBase.Type = DatabaseType.SingleFamily;
            this.Close();
        }

        private void btnTownhome_Click(object sender, EventArgs e)
        {
            DataBase.Type = DatabaseType.Townhome;
            this.Close();
        }

        private void btnCarriage_Click(object sender, EventArgs e)
        {
            DataBase.Type = DatabaseType.Carriage;
            this.Close();
        }

        private void btn_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackgroundImage = PlanReferenceDatabase.Properties.Resources.SFButtonHover;
        }

        private void btn_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackgroundImage = PlanReferenceDatabase.Properties.Resources.SFButton1;
        }
    }
}
