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
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(WorkoutModel workoutModel)
        {
            if (ModelState.IsValid)
            {
                return View("CreateWorkout", workoutModel);
            }
            return View();            
        }

        
    }
}