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
            VerifyAuthentication();

            // Create ColumnWidths Collection if neccessary
            if (MilbrandtFPDB.Properties.Settings.Default.ColumnWidths == null)
                MilbrandtFPDB.Properties.Settings.Default.ColumnWidths = new System.Collections.Specialized.StringCollection();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Save user settings
            MilbrandtFPDB.Properties.Settings.Default.Save();
        }

        private void VerifyAuthentication()
        {
            try
            {
                if (!SecurityHelper.CheckAuthorization())
                {
                    // prevent the app from terminating after the password window closes
                    Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;

                    PasswordWindow pw = new PasswordWindow();
                    bool? result = pw.ShowDialog();

                    if (result.HasValue && result.Value)
                    {
                        SecurityHelper.AuthorizeCurrent();

                        // return app to normal shutdown mode
                        Current.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
                    }
                    else
                    {
                        this.Shutdown();
                    }
                }
            }
            catch (System.Security.SecurityException)
            {
                MessageBox.Show("The application does not have priviledge to check or set authentication, and cannot continue.");
                this.Shutdown();
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.Message == "The system cannot find the file specified")
                {
                    MessageBox.Show("Cannot locate authorization exe. The program will continue, but authorization has not been set.");
                    Current.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
                }
                else
                {
                    MessageBox.Show("The application does not have priviledge to check or set authentication, and cannot continue.");
                    this.Shutdown();
                }
            }
        }
    }
}
