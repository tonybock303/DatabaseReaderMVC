using DatabaseReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseReader.Helpers
{
    public static class ChartSeriesHelper
    {
        // The number of minutes in one day
        public static readonly int _minsInOneDay = 1440;
        public static List<double[]> GetCollection(DateTime selectedDate, int rangeInDays, List<History> history, ReturnSeriesOf returnDataSetType)
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
        public static double[] GetAverage(List<double[]> collectionOfDaysCalPerMin)
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

        public static void CalculateCalsBurntPerMinBetweenInputs(DateTime date, List<History> history)
        {
            List<History> his = history.Where(x => x.IsSameDate(date)).ToList();
            for (int i = 1; i < his.Count; i++)
            {
                double caloriesBurntBetweenInputs = his[i - 1].CaloriesBurnt - his[i].CaloriesBurnt;
                double minsDuringInterval = (his[i - 1].DateTime - his[i].DateTime).TotalMinutes;
                his[i - 1].CaloriesBurntPerMin = Math.Round(caloriesBurntBetweenInputs / minsDuringInterval, 3);
            }
        }
        public static double CalculateCaloriesPerMin(History[] historyEntries, int historyCount)
        {
            return (historyEntries[historyCount].CaloriesBurnt - historyEntries[historyCount - 1].CaloriesBurnt)
                / (historyEntries[historyCount].DateTime - historyEntries[historyCount - 1].DateTime).TotalMinutes;
        }

        public static string[] CalculateTimeSeries()
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
        public static double[] CalculateAverageSeries(DateTime selectedDate, int rangeInDays, List<History> history, ReturnSeriesOf returnSeriesOf)
        {
            List<double[]> collectionOfDays = GetCollection(selectedDate, rangeInDays, history, returnSeriesOf);
            return GetAverage(collectionOfDays);
        }

        public static double[] CalculateSeries(DateTime date, List<History> history, ReturnSeriesOf returnSeriesOf)
        {
            double[] series = new double[_minsInOneDay];
            DateTime timeCounter = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0) ;
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

    }
}