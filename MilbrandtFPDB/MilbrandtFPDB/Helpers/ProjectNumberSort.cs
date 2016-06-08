using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MilbrandtFPDB
{
    public class ProjectNumberSort : System.Collections.IComparer
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

            int sortFactor = _sortDirection == ListSortDirection.Descending ? 1 : -1;

            if (spx == null && spy == null)
                return 0;
            else if (spx == null)
                return 1 * sortFactor;
            else if (spy == null)
                return -1 * sortFactor;

            if (String.IsNullOrWhiteSpace(spx.ProjectNumber) && String.IsNullOrWhiteSpace(spy.ProjectNumber))
                return 0;
            else if (String.IsNullOrWhiteSpace(spx.ProjectNumber))
                return 1 * sortFactor;
            else if (String.IsNullOrWhiteSpace(spy.ProjectNumber))
                return -1 * sortFactor;

            StringBuilder sbx = new StringBuilder(spx.ProjectNumber);
            StringBuilder sby = new StringBuilder(spy.ProjectNumber);
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
}
