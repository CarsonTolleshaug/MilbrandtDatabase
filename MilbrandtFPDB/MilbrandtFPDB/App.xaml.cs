using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MilbrandtFPDB
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Create ColumnWidths Collection if neccessary
            if (MilbrandtFPDB.Properties.Settings.Default.ColumnWidths == null)
                MilbrandtFPDB.Properties.Settings.Default.ColumnWidths = new System.Collections.Specialized.StringCollection();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Save user settings
            MilbrandtFPDB.Properties.Settings.Default.Save();
        }
    }
}
