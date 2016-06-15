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
                Assert.AreEqual((i + 1).ToString(), plans[i].ProjectName);
            }
        }

        [TestMethod]
        public void AvailableValuesTest()
        {
            const int NUM_ENTRIES = 4;

            string[] projectNumbers =   { "1111", "1111", "0222", "9999" };
            string[] projectNames =     { "AAAA", "BBBB", "AAAA", "AAAA" };
            string[] locations =        { "XXXX", "XXXX", "YYYY", "ZZZZ" };
            string[] squareFeets =      { "1025", "2025", "2000", "501"  };
            string[] dates = { "01/10/2014", "01/09/2016", "03/10/2014", "12/15/2015" };


            List<SitePlan> plans = new List<SitePlan>();
            for (int i = 0; i < NUM_ENTRIES; i++)
            {
                plans.Add(new SitePlan() {
                    ProjectNumber = projectNumbers[i],
                    ProjectName = projectNames[i],
                    Location = locations[i],
                    SquareFeet = squareFeets[i],
                    Date = DateTime.Parse(dates[i])
                });
            }

            MainWindowViewModel vm = new MainWindowViewModel(plans);

            //Assert.AreEqual()
        }

        [TestMethod]
        public void FiltersTest()
        {

        }

        [TestMethod]
        public void AddEntryTest()
        {

        }

        [TestMethod]
        public void EditSingleEntryTest()
        {

        }

        [TestMethod]
        public void EditMultipleEntriesTest()
        {

        }
    }
}
