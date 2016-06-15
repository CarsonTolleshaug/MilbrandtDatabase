using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MilbrandtFPDB;
using System.Collections.Generic;

namespace FPDBUnitTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void ProjectNumberSortOrderTest()
        {    
            ProjectNumberSort pns = new ProjectNumberSort(System.ComponentModel.ListSortDirection.Ascending);
            SitePlan sp1, sp2, sp3;

            // Standard sorting
            sp1 = new SitePlan() { ProjectNumber = "0210" };
            sp2 = new SitePlan() { ProjectNumber = "1610" };
            Assert.IsTrue(pns.Compare(sp1, sp2) < 0, "Project Number Sorting does not sort in the proper direction");
            Assert.IsTrue(pns.Compare(sp2, sp1) > 0, "Project Number Sorting does not sort in the proper direction");

            sp1 = new SitePlan() { ProjectNumber = "9310" };
            sp2 = new SitePlan() { ProjectNumber = "9910" };
            Assert.IsTrue(pns.Compare(sp1, sp2) < 0, "Project Number Sorting does not sort in the proper direction");
            Assert.IsTrue(pns.Compare(sp2, sp1) > 0, "Project Number Sorting does not sort in the proper direction");

            // Return 0 on equal
            Assert.IsTrue(pns.Compare(sp1, sp1) == 0, "Project Number Sorting does not return 0 for equal siteplans");

            // Year-like sorting
            sp1 = new SitePlan() { ProjectNumber = "9810" };
            sp2 = new SitePlan() { ProjectNumber = "0210" };
            Assert.IsTrue(pns.Compare(sp1, sp2) < 0, "Project Number Sorting does not put \"98**\" (1998) before \"02**\" (2002)");
            Assert.IsTrue(pns.Compare(sp2, sp1) > 0, "Project Number Sorting does not put \"98**\" (1998) before \"02**\" (2002)");

            // Sub-sorting by plan
            sp1 = new SitePlan() { ProjectNumber = "9810", Plan = "Plan 2A" };
            sp2 = new SitePlan() { ProjectNumber = "9810", Plan = "Plan 3B" };
            sp3 = new SitePlan() { ProjectNumber = "9810", Plan = "Plan 10A" };
            Assert.IsTrue(pns.Compare(sp1, sp2) < 0, "Project Number Sorting does not sub-sort by plan");
            Assert.IsTrue(pns.Compare(sp2, sp3) < 0, "Project Number Sorting does not sub-sort by plan");
            Assert.IsTrue(pns.Compare(sp1, sp3) < 0, "Project Number Sorting does not sub-sort by plan");
            Assert.IsTrue(pns.Compare(sp2, sp1) > 0, "Project Number Sorting does not sub-sort by plan");
            Assert.IsTrue(pns.Compare(sp3, sp2) > 0, "Project Number Sorting does not sub-sort by plan");
            Assert.IsTrue(pns.Compare(sp3, sp1) > 0, "Project Number Sorting does not sub-sort by plan");

            // Lastly, do a general test which should double check everything
            List<SitePlan> plans = new List<SitePlan>() {
                new SitePlan() { ProjectNumber = "9810", Plan = "Plan 10A", ProjectName = "4"},
                new SitePlan() { ProjectNumber = "9910", Plan = "Plan 4", ProjectName = "5"},
                new SitePlan() { ProjectNumber = "9810", Plan = "Plan 2B", ProjectName = "3"},
                new SitePlan() { ProjectNumber = "0210", Plan = "Plan A", ProjectName = "6"},
                new SitePlan() { ProjectNumber = "2210", Plan = "Plan A", ProjectName = "7"},
                new SitePlan() { ProjectNumber = "9215", Plan = "Plan A", ProjectName = "1"},
                new SitePlan() { ProjectNumber = "9215", Plan = "Plan B", ProjectName = "2"}
            };

            plans.Sort(pns);
            for (int i = 0; i < plans.Count; i++)
            {
                // The project name is the sort order (starting at 1)
                Assert.AreEqual(plans[i].ProjectName, (i + 1).ToString());
            }
        }
    }
}
