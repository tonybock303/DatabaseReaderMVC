using DatabaseReader.Data;
using DatabaseReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DatabaseReader.Controllers
{
    public class DetailsController : Controller
    {
        // Our database
        private static IDatabase db = new DataAccess();
        // The number of minutes in one day
        private static readonly int _minsInOneDay = 1440;
        // How many days to look back when calculating the average of a given day
        private static readonly int rangeInDays = 100;

        public ActionResult Details(string id)
        {
            DateTime selectedDate;
            DateTime.TryParse(id, out selectedDate);
            if (string.IsNullOrEmpty(id) || selectedDate == null)
            {
                return RedirectToAction("Days", "Days");
            }

            List<History> history = GetAllHistory();
            CalculateCalsBurntPerMinBetweenInputs(selectedDate, history);

            
            var model = history.Where(x => x.IsSameDate(selectedDate)).OrderByDescending(x => x.DateTime).ToList();

            // Calculate the time series
            ViewBag.TIME = CalculateTimeSeries();
            // Calculate the selected date series
            ViewBag.CALS = CalculateSeries(selectedDate, history, ReturnSeriesOf.CaloriesBurnt);
            // Calculate the same day of last week series            
            ViewBag.LAST = CalculateSeries(selectedDate.AddDays(-7), history, ReturnSeriesOf.CaloriesBurnt);
            // Calculate the average of this day series
            ViewBag.AVG = CalculateAverageSeries(selectedDate, rangeInDays, history, ReturnSeriesOf.CaloriesBurnt);
            // Calculate the average calories per min series
            ViewBag.CPM = CalculateSeries(selectedDate, history, ReturnSeriesOf.CaloriesPerMin);
            // Calculate the calories per min series
            ViewBag.AVGCPM = CalculateAverageSeries(selectedDate, rangeInDays, history, ReturnSeriesOf.CaloriesPerMin);

            return View("Details", model);
        }

        private static double[] CalculateAverageSeries(DateTime selectedDate, int rangeInDays, List<History> history, ReturnSeriesOf returnSeriesOf)
        {
            
            List<double[]> collectionOfDays = GetCollection(selectedDate, rangeInDays, history, returnSeriesOf);                        
            return GetAverage(collectionOfDays);
        }
        private static List<double[]> GetCollection(DateTime selectedDate, int rangeInDays, List<History> history, ReturnSeriesOf returnDataSetType)
        {
            List<double[]> collectionOfDays = new List<double[]>();
            // Get previous days to calculate average
            // Only select those that are the same day of the week
            // Only look back as far as the rangeInDays
            // Not including the selected day
            for (int i = 1; i < rangeInDays; i++)
            {
                DateTime date = selectedDate.AddDays(-i);
                if (date.DayOfWeek == selectedDate.DayOfWeek)
                {
                    List<History> currentHistory = new List<History>();
                    currentHistory = history.Where(x => x.IsSameDate(date)).OrderBy(x => x.DateTime).ToList();
                    if (currentHistory.Count > 0)
                    {
                        collectionOfDays.Add(CalculateSeries(date, history, returnDataSetType));
                    }
                    
                }
            }
            return collectionOfDays;
        }
        private static double[] GetAverage(List<double[]> collectionOfDaysCalPerMin)
        {
            double[] averageOfThisDay = new double[_minsInOneDay];
            for (int i = 0; i < _minsInOneDay; i++)
            {
                double count = 0;
                foreach (double[] series in collectionOfDaysCalPerMin)
                {
                    count += series[i];
                }
                double avg = count / collectionOfDaysCalPerMin.Count;
                averageOfThisDay[i] = avg;
            }
            return averageOfThisDay;
        }

        private static void CalculateCalsBurntPerMinBetweenInputs(DateTime date, List<History> history)
        {
            List<History> his = history.Where(x => x.IsSameDate(date)).ToList();
            for (int i = 1; i < his.Count; i++)
            {
                double caloriesBurntBetweenInputs = his[i - 1].CaloriesBurnt - his[i].CaloriesBurnt;
                double minsDuringInterval = (his[i - 1].DateTime - his[i].DateTime).TotalMinutes;
                his[i - 1].CaloriesBurntPerMin = Math.Round(caloriesBurntBetweenInputs / minsDuringInterval, 3);
            }
        }
        private static double CalculateCaloriesPerMin(History[] historyEntries, int historyCount)
        {
            return (historyEntries[historyCount].CaloriesBurnt - historyEntries[historyCount - 1].CaloriesBurnt)
                / (historyEntries[historyCount].DateTime - historyEntries[historyCount - 1].DateTime).TotalMinutes;
        }
        private static double[] CalculateSeries(DateTime date, List<History> history, ReturnSeriesOf returnSeriesOf)
        {
            
            double[] series = new double[_minsInOneDay];
            DateTime timeCounter = date;
            History[] historyEntries = history.Where(x => x.IsSameDate(date)).OrderBy(x => x.DateTime).ToArray();
            if (historyEntries.Length == 0)
            {
                return new double[_minsInOneDay];
            }

            int historyCount = 1; //skip the first element at 00:00 of 0
            double calsPerMin = CalculateCaloriesPerMin(historyEntries, historyCount);

            double caloriesBurnt = 0;

            // itterate through the minutes of the day
            for (int i = 0; i < _minsInOneDay; i++)
            {
                if (historyCount < historyEntries.Length - 1)
                {
                    if (historyEntries[historyCount].IsSameTimeInMins(timeCounter))
                    {
                        historyCount++;
                        calsPerMin = CalculateCaloriesPerMin(historyEntries, historyCount);
                    }
                }
                switch (returnSeriesOf)
                {
                    case ReturnSeriesOf.CaloriesBurnt:
                        caloriesBurnt += calsPerMin;
                        series[i] = caloriesBurnt;
                        break;
                    case ReturnSeriesOf.CaloriesPerMin:
                        series[i] = calsPerMin;
                        break;
                    default:
                        break;
                }
                timeCounter = timeCounter.AddMinutes(1);

                if (timeCounter > DateTime.Now) break;
            }
            return series;
        }
        private static string[] CalculateTimeSeries()
        {
            string[] vals = new string[_minsInOneDay];
            DateTime counter = new DateTime();

            for (int i = 0; i < _minsInOneDay; i++)
            {
                vals[i] = counter.ToString("HH:mm");
                counter = counter.AddMinutes(1);
            }
            return vals;
        }
        private static List<History> GetHistory(DateTime date)
        {
            return db.GetAllHistories().OrderByDescending(x => x.DateTime).Where(x => x.IsSameDate(date)).ToList();
        }

        private static List<History> GetAllHistory()
        {
            return db.GetAllHistories().OrderByDescending(x => x.DateTime).ToList();
        }
    }

    enum ReturnSeriesOf
    {
        CaloriesBurnt,
        CaloriesPerMin
    }
}