using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseReader.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DatabaseReader.Models;
using DatabaseReaderTests.Controllers;

namespace DatabaseReader.Controllers.Tests
{
    [TestClass()]
    public class DaysControllerTests
    {
        [TestMethod()]
        public void DaysTest()
        {
            TestDataAccess tda = new TestDataAccess();
            DaysController controler = new DaysController(tda);
            ViewResult view = controler.Days() as ViewResult;
            var temp = view.ViewData.Model as IEnumerable<DatabaseReader.Models.Day>;
            foreach (Day day in temp)
            {
                Assert.IsTrue(day.TotalCalories > 0);
            }
            Assert.IsNotNull(view);            
        }
    }
}