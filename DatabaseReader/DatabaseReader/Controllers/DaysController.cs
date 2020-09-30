using DatabaseReader.Data;
using DatabaseReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DatabaseReader.Controllers
{
    public class DaysController : Controller
    {
        // Our database
        private static IDatabase db = new DataAccess();

        public ActionResult Days()
        {
            IEnumerable<Day> days = db.GetAllDays().OrderByDescending(x => x.DateTime);
            IEnumerable<History> histories = db.GetAllHistories()
                .OrderByDescending(x => x.DateTime);

            // Populate total calories from history
            foreach (Day day in days)
            {
                day.TotalCalories = histories.Where(x => x.IsSameDate(day.DateTime))
                    .OrderByDescending(x => x.DateTime).First().CaloriesBurnt;                
            }
            var model = days;

            return View(model);
        }

        public ActionResult Details(string id)
        {
            return RedirectToAction("Details", "Details", new { id });
        }




    }
}