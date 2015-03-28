using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;



namespace HyperlinkJobsList
{
    public class Job
    {
        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        private static IntPtr prcId;
        private static int winHWND;
        private static int prcHWND;
        private static int threadID;
        private static bool openWindow = false;

        //Instance variable properties
        public string JobNumber
        {
            get;
            set;
        }

        public string ProjectTitle
        {
            get;
            set;
        }
        
        public string ClientName
        {
            get;
            set;
        }

        public string FileLocation
        {
            get;
            set;
        }

        public string Assigned
        {
            get;
            set;
        }

        public string Alias
        {
            get;
            set;
        }

        public string Drawer
        {
            get;
            set;
        }

        public Job(string job_number, string project_title, string client_name, string file_location, string emps_assigned, string alias, string drawer)
        {
            JobNumber = job_number;
            ProjectTitle = project_title;
            ClientName = client_name;
            FileLocation = file_location;
            Assigned = emps_assigned;
            Alias = alias;
            Drawer = drawer;
        }

        public bool IsEqualTo(Job other)
        {
            if (!this.JobNumber.Equals(other.JobNumber))
                return false;
            if (!this.ProjectTitle.Equals(other.ProjectTitle))
                return false;
            if (!this.ClientName.Equals(other.ClientName))
                return false;
            if (!this.Assigned.Equals(other.Assigned))
                return false;
            if (!this.Alias.Equals(other.Alias))
                return false;
            if (!this.FileLocation.Equals(other.FileLocation))
                return false;
            if (!this.Drawer.Equals(other.Drawer))
                return false;
            
            return true;
        }
        
        public bool ContainsQuery(string query)
        {
            //removes case-sensitivity
            query = query.ToLower();

            if (JobNumber.ToLower().Contains(query))
                return true;
            if (ProjectTitle.ToLower().Contains(query))
                return true;
            if (ClientName.ToLower().Contains(query))
                return true;
            if (Assigned.ToLower().Contains(query))
                return true;
            if (Alias.ToLower().Contains(query))
                return true;
            if (Drawer.ToLower().Contains(query))
                return true;

            return false;
        }

        public void Open()
        {
            if (openWindow)
            {
                try
                {
                    SHDocVw.ShellWindows windows = new SHDocVw.ShellWindows();

                    for (int i = 0; i < windows.Count; ++i)
                    {
                        object t = windows.Item(i);
                        if (t != null)
                        {
                            SHDocVw.InternetExplorer window = (SHDocVw.InternetExplorer)windows.Item(i);
                            //int winThreadId = (int)GetWindowThreadProcessId((IntPtr)window.HWND, prcId);
                            int test = window.HWND;
                            test = winHWND;
                            if (winHWND == window.HWND)
                            {
                                
                                object Flags = new object();
                                object TargetFrame = new object();
                                object PostData = new object();
                                object Headers = new object();

                                window.Navigate(FileLocation, ref Flags, ref TargetFrame, ref PostData, ref Headers);
                                // make the window active
                                window.Visible = true;
                                return;
                            }
                        }
                    }
                }
                catch { } //Do nothing, because we can just have it open another window if this doesn't work.
                // The following code will execute if either the window was not found, or there was an exception
                openWindow = false;
                Open(); //Retry open() the normal way, by starting a process
            }
            else
            {
                //If it could not be found, make a new one...
                string windir = Environment.GetEnvironmentVariable("WINDIR");
                System.Diagnostics.Process prc = new System.Diagnostics.Process();
                prc.StartInfo.FileName = windir + @"\explorer.exe";
                prc.StartInfo.Arguments = FileLocation;


                List<int> HWNDs = new List<int>();
                try
                {
                    //Before we start the process, find all instances of explorer open:
                    SHDocVw.ShellWindows windows = new SHDocVw.ShellWindows();
                    for (int i = 0; i < windows.Count; ++i)
                    {
                        object t = windows.Item(i);
                        if (t != null)
                        {
                            HWNDs.Add(((SHDocVw.InternetExplorer)windows.Item(i)).HWND);
                        }
                    }
                }
                catch 
                {
                    prc.Start();
                    return; //no need to do the rest otherwise
                }
                


                prc.Start();



                try
                {
                    for (int x = 0; x < 10; ++x) //Try this every 200ms for a maximum of 2 seconds before giving up
                    {
                        System.Threading.Thread.Sleep(200); //wait a bit for the window to get set up
                        SHDocVw.ShellWindows windows = new SHDocVw.ShellWindows();
                        for (int i = 0; i < windows.Count; ++i)
                        {
                            object t = windows.Item(i);
                            if (t != null)
                            {
                                SHDocVw.InternetExplorer window = (SHDocVw.InternetExplorer)windows.Item(i);
                                if (!HWNDs.Contains(window.HWND))
                                {
                                    openWindow = true;
                                    winHWND = window.HWND;
                                    return;
                                }
                            }
                        }
                    }
                }
                catch { }
            }
        }

        public string Writestring
        {
            get
            {
                return JobNumber + "|" + ProjectTitle + "|" + ClientName + "|" + FileLocation + "|" + Assigned + "|" + Alias + "|" + Drawer;
            }
        }
    }
}
