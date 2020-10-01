using DatabaseReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DatabaseReader.Controllers
{
    public class WorkoutController : Controller
    {
        // GET: Workout
        [HttpGet]
        public ActionResult CreateWorkout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWorkout(WorkoutModel workoutModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("DisplayWorkout", "Workout", workoutModel);
            }
            return View();            
        }

        public ActionResult DisplayWorkout(WorkoutModel workoutModel)
        {
            return View(workoutModel);
        }
    }
}