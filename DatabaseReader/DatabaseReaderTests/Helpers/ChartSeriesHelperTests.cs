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
using DatabaseReaderTests;

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
            List<double[]> collection = ChartSeriesHelper.GetCollection(today, TestSettings.rangeInDays, history, ReturnSeriesOf.CaloriesBurnt);

            Random r = new Random();
            int index = r.Next(1439);
            double avgIndex = new double();
            foreach (double[] series in collection)
            {
                avgIndex += series[index];
            }
            avgIndex = avgIndex / collection.Count;

            double[] average = ChartSeriesHelper.GetAverage(collection);

            Assert.IsTrue(IsCalories(average));
            Assert.IsFalse(IsCalsPerMin(average));

            Assert.IsTrue(average[index] == avgIndex);
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
            int count = average.Where(x => x == 0).Count();
            if (count > 1) isCalories = false;
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
            calsPerMin = ChartSeriesHelper.CalculateCaloriesPerMin(todaysHistory, todaysHistory.Length - 1);
            Assert.IsTrue(calsPerMin > 0);
            calsPerMin = ChartSeriesHelper.CalculateCaloriesPerMin(todaysHistory, 2);
            Assert.IsTrue(calsPerMin > 0);            
        }

        [TestMethod()]
        public void CalculateTimeSeriesTest()
        {
            string[] timeSeries = ChartSeriesHelper.CalculateTimeSeries();
            DateTime? lastTime = null;
            foreach (string str in timeSeries)
            {
                var temp = str.Substring(3, 2);
                int hour = int.Parse(str.Substring(0, 2));
                int min = int.Parse(str.Substring(3, 2));
                Assert.IsTrue(hour <= 23);
                Assert.IsTrue(min <= 59);
                DateTime time = new DateTime(today.Year, today.Month, today.Day, hour, min, 0);
                if (lastTime != null)
                {
                    Assert.IsTrue(lastTime < time);
                }
                lastTime = new DateTime(time.Year, time.Month, time.Day, hour, min, 0);
            }
        }

        [TestMethod()]
        public void CalculateAverageSeriesTest()
        {
            double[] averageCalories = ChartSeriesHelper.CalculateAverageSeries(today, TestSettings.rangeInDays, history, ReturnSeriesOf.CaloriesBurnt);
            double[] averageCalsPerMin = ChartSeriesHelper.CalculateAverageSeries(today, TestSettings.rangeInDays, history, ReturnSeriesOf.CaloriesPerMin);
            Assert.IsTrue(IsCalories(averageCalories));
            Assert.IsTrue(IsCalsPerMin(averageCalsPerMin));
        }

        [TestMethod()]
        public void CalculateSeriesTest()
        {
            double[] caloriesSeries = ChartSeriesHelper.CalculateSeries(today.AddDays(-1), history, ReturnSeriesOf.CaloriesBurnt);
            double[] calsPerMinSeries = ChartSeriesHelper.CalculateSeries(today.AddDays(-1), history, ReturnSeriesOf.CaloriesPerMin);
            Assert.IsTrue(IsCalories(caloriesSeries));
            Assert.IsTrue(IsCalsPerMin(calsPerMinSeries));
        }
        [TestMethod()]
        public void CalculateSeriesTodayTest()
        {
            double[] caloriesSeries = ChartSeriesHelper.CalculateSeries(today, history, ReturnSeriesOf.CaloriesBurnt);
           
            int mins = (today.Hour * 60) + (today.Minute);
            Assert.IsTrue(caloriesSeries[mins + 10] == 0);
        }
    }
}