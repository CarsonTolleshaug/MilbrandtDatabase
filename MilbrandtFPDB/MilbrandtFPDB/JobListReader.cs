using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MilbrandtFPDB
{
    public static class JobListReader
    {
        public static string JobListFile = @"C:\Users\carso\Documents\Milbrandt\MilbrandtDatabase\JobsList\Milbrandt Job List\bin\x86\Release\jobs.dat";

        public static Dictionary<string, string> GetJobInfo(string projectNumber)
        {            
            if (File.Exists(JobListFile))
            {
                StreamReader sr = new StreamReader(JobListFile);

                while (!sr.EndOfStream)
                {
                    string[] data = sr.ReadLine().Split("|".ToCharArray());
                    
                    // old files have < 7 params, data[0] is the project number
                    if (data.Length == 7 && data[0] == projectNumber)
                    {
                        sr.Close();
                        Dictionary<string, string> info = new Dictionary<string, string>();

                        info.Add("ProjectNumber", data[0]);
                        info.Add("ProjectName", data[1]);
                        info.Add("ClientName", data[2]);
                        info.Add("DirectoryLocation", data[3]);
                        info.Add("EmployeesAssigned", data[4]);
                        info.Add("Alias", data[5]);
                        info.Add("Drawer", data[6]);

                        return info;
                    }
                }

                sr.Close();
            }

            return null;
        }
    }
}
