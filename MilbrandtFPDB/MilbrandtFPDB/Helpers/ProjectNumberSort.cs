using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MilbrandtFPDB
{
    public class ProjectNumberSort : System.Collections.IComparer, IComparer<string>
    {
        private const int MAX_YEAR = 80; // 1980
        private ListSortDirection _sortDirection;

        public ProjectNumberSort(ListSortDirection sortDirection)
        {
            _sortDirection = sortDirection;
        }

        public int Compare(object x, object y)
        {
            SitePlan spx = x as SitePlan;
            SitePlan spy = y as SitePlan;

            int sortFactor = _sortDirection == ListSortDirection.Ascending ? 1 : -1;

            if (spx == null && spy == null)
                return 0;
            else if (spx == null)
                return 1 * sortFactor;
            else if (spy == null)
                return -1 * sortFactor;

            int result = Compare(spx.ProjectNumber, spy.ProjectNumber);
            
            // Do secondary sorting by plot
            if (result == 0)
            {
                AlphanumComparatorFast acf = new AlphanumComparatorFast();
                return acf.Compare(spx.Plan, spy.Plan);
            }

            return result;
        }

        public int Compare(string x, string y)
        {
            int sortFactor = _sortDirection == ListSortDirection.Ascending ? 1 : -1;

            if (String.IsNullOrWhiteSpace(x) && String.IsNullOrWhiteSpace(y))
                return 0;
            else if (String.IsNullOrWhiteSpace(x))
                return 1 * sortFactor;
            else if (String.IsNullOrWhiteSpace(y))
                return -1 * sortFactor;

            StringBuilder sbx = new StringBuilder(x);
            StringBuilder sby = new StringBuilder(y);
            int temp;


            if (int.TryParse(sbx.ToString(), out temp))
            {
                // anything where the first two numbers are >= 80
                if (temp >= MAX_YEAR * 100)
                {
                    // add 19 to beginning (e.g. 8018 becomes 198018, since 1980 is the year)
                    sbx.Insert(0, "19");
                }
                else
                {
                    // otherwise add 20 to the beginning (e.g. 0644 becomes 200644, 2006 is the year)
                    sbx.Insert(0, "20");
                }
            }

            if (int.TryParse(sby.ToString(), out temp))
            {
                if (temp >= MAX_YEAR * 100)
                {
                    sby.Insert(0, "19");
                }
                else
                {
                    sby.Insert(0, "20");
                }
            }

            return string.Compare(sbx.ToString(), sby.ToString()) * sortFactor;
        }
    }




    // From StackOverflow: http://stackoverflow.com/questions/5093842/alphanumeric-sorting-using-linq
    public class AlphanumComparatorFast : System.Collections.IComparer
    {
        List<string> GetList(string s1)
        {
            List<string> SB1 = new List<string>();
            string st1;
            st1 = "";
            bool flag = char.IsDigit(s1[0]);
            foreach (char c in s1)
            {
                if (flag != char.IsDigit(c) || c == '\'')
                {
                    if (st1 != "")
                        SB1.Add(st1);
                    st1 = "";
                    flag = char.IsDigit(c);
                }
                if (char.IsDigit(c))
                {
                    st1 += c;
                }
                if (char.IsLetter(c))
                {
                    st1 += c;
                }


            }
            SB1.Add(st1);
            return SB1;
        }



        public int Compare(object x, object y)
        {
            string s1 = x as string;
            if (s1 == null)
            {
                return 0;
            }
            string s2 = y as string;
            if (s2 == null)
            {
                return 0;
            }
            if (s1 == s2)
            {
                return 0;
            }
            int len1 = s1.Length;
            int len2 = s2.Length;

            // Walk through two the strings with two markers.
            List<string> str1 = GetList(s1);
            List<string> str2 = GetList(s2);
            while (str1.Count != str2.Count)
            {
                if (str1.Count < str2.Count)
                {
                    str1.Add("");
                }
                else
                {
                    str2.Add("");
                }
            }
            int x1 = 0; int res = 0; int x2 = 0; string y2 = "";
            bool status = false;
            string y1 = ""; bool s1Status = false; bool s2Status = false;
            //s1status ==false then string ele int;
            //s2status ==false then string ele int;
            int result = 0;
            for (int i = 0; i < str1.Count && i < str2.Count; i++)
            {
                status = int.TryParse(str1[i].ToString(), out res);
                if (res == 0)
                {
                    y1 = str1[i].ToString();
                    s1Status = false;
                }
                else
                {
                    x1 = Convert.ToInt32(str1[i].ToString());
                    s1Status = true;
                }

                status = int.TryParse(str2[i].ToString(), out res);
                if (res == 0)
                {
                    y2 = str2[i].ToString();
                    s2Status = false;
                }
                else
                {
                    x2 = Convert.ToInt32(str2[i].ToString());
                    s2Status = true;
                }
                //checking --the data comparision
                if (!s2Status && !s1Status)    //both are strings
                {
                    result = str1[i].CompareTo(str2[i]);
                }
                else if (s2Status && s1Status) //both are intergers
                {
                    if (x1 == x2)
                    {
                        if (str1[i].ToString().Length < str2[i].ToString().Length)
                        {
                            result = 1;
                        }
                        else if (str1[i].ToString().Length > str2[i].ToString().Length)
                            result = -1;
                        else
                            result = 0;
                    }
                    else
                    {
                        int st1ZeroCount = str1[i].ToString().Trim().Length - str1[i].ToString().TrimStart(new char[] { '0' }).Length;
                        int st2ZeroCount = str2[i].ToString().Trim().Length - str2[i].ToString().TrimStart(new char[] { '0' }).Length;
                        if (st1ZeroCount > st2ZeroCount)
                            result = -1;
                        else if (st1ZeroCount < st2ZeroCount)
                            result = 1;
                        else
                            result = x1.CompareTo(x2);

                    }
                }
                else
                {
                    result = str1[i].CompareTo(str2[i]);
                }
                if (result == 0)
                {
                    continue;
                }
                else
                    break;

            }
            return result;
        }
    }
}
