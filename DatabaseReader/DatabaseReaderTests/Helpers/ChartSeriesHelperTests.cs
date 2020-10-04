using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseReader.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseReaderTests.Controllers;
using DatabaseReader.Models;
using System.Security.Cryptography.X509Certificates;

namespace DatabaseReader.Helpers.Tests
{
    [TestClass()]
    public class ChartSeriesHelperTests
    {
        TestDataAccess tda = new TestDataAccess();
        List<History> history;
        DateTime today;

        [TestInitialize]
        public void SetUp()
        {
            history = tda.GetAllHistories();
            today = DateTime.Now;
        }


        [TestMethod()]
        public void GetCollectionTest()
        {

            List<double[]> collection = ChartSeriesHelper.GetCollection(today, 100, history, ReturnSeriesOf.CaloriesBurnt);
            int numberOfRecords = 100 / 7;
            Assert.IsTrue(collection.Count == numberOfRecords);

            collection = ChartSeriesHelper.GetCollection(today, 100, history, ReturnSeriesOf.CaloriesPerMin);
            foreach (double[] series in collection)
            {
                Assert.IsTrue(series.Where(x => x > 20).Count() == 0);
            }

        }

        [TestMethod()]
        public void GetAverageTest()
        {
            List<double[]> collection = ChartSeriesHelper.GetCollection(today, 100, history, ReturnSeriesOf.CaloriesBurnt);

            double[] average = ChartSeriesHelper.GetAverage(collection);

            Assert.IsTrue(IsCalories(average));
            Assert.IsFalse(IsCalsPerMin(average));
        }

        private bool IsCalsPerMin(double[] average)
        {
            bool isCalsPerMin = true;
            if (average.Where(x => x > 20).Count() > 0) isCalsPerMin = false;
            return isCalsPerMin;
        }

        private bool IsCalories(double[] average)
        {
            bool isCalories = true;
            if (average.Where(x => x == 0).Count() > 1) isCalories = false;
            return isCalories;
        }

        [TestMethod()]
        public void CalculateCalsBurntPerMinBetweenInputsTest()
        {
            List<History> todaysHistory = history.Where(x => x.IsSameDate(today)).ToList();
            Assert.IsTrue(todaysHistory.Where(x => x.CaloriesBurntPerMin != 0).Count() == 0);
            ChartSeriesHelper.CalculateCalsBurntPerMinBetweenInputs(today, todaysHistory);
            Assert.IsTrue(todaysHistory.Where(x => x.CaloriesBurntPerMin == 0).Count() == 1);
        }

        [TestMethod()]
        public void CalculateCaloriesPerMinTest()
        {
            History[] todaysHistory = history.Where(x => x.IsSameDate(today)).ToArray();
            double calsPerMin;
            calsPerMin = ChartSeriesHelper.CalculateCaloriesPerMin(todaysHistory, 1);
            Assert.IsTrue(calsPerMin > 0);
            calsPerMin = ChartSeriesHelper.CalculateCaloriesPerMin(todaysHistory, todaysHistory.Length-1);
            Assert.IsTrue(calsPerMin > 0);
            calsPerMin = ChartSeriesHelper.CalculateCaloriesPerMin(todaysHistory, 2);
            Assert.IsTrue(calsPerMin > 0);
        }
    }
}