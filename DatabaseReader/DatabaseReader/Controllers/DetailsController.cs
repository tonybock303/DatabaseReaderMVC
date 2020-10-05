using DatabaseReader.Data;
using DatabaseReader.Helpers;
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
            ChartSeriesHelper.CalculateCalsBurntPerMinBetweenInputs(selectedDate, history);

            
            var model = history.Where(x => x.IsSameDate(selectedDate)).OrderByDescending(x => x.DateTime).ToList();

            // Calculate the time series
            ViewBag.TIME = ChartSeriesHelper.CalculateTimeSeries();
            // Calculate the selected date series
            ViewBag.CALS = ChartSeriesHelper.CalculateSeries(selectedDate, history, ReturnSeriesOf.CaloriesBurnt);
            // Calculate the same day of last week series            
            ViewBag.LAST = ChartSeriesHelper.CalculateSeries(selectedDate.AddDays(-7), history, ReturnSeriesOf.CaloriesBurnt);
            // Calculate the average of this day series
            ViewBag.AVG = ChartSeriesHelper.CalculateAverageSeries(selectedDate, rangeInDays, history, ReturnSeriesOf.CaloriesBurnt);
            // Calculate the average calories per min series
            ViewBag.CPM = ChartSeriesHelper.CalculateSeries(selectedDate, history, ReturnSeriesOf.CaloriesPerMin);
            // Calculate the calories per min series
            ViewBag.AVGCPM = ChartSeriesHelper.CalculateAverageSeries(selectedDate, rangeInDays, history, ReturnSeriesOf.CaloriesPerMin);

            return View("Details", model);
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

    
}