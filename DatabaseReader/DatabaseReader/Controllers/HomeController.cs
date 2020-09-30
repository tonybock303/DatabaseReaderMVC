using DatabaseReader.Data;
using DatabaseReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DatabaseReader.Controllers
{
    public class HomeController : Controller
    {
        private List<Day> days = new List<Day>();
        public ActionResult Index()
        {
            return View();
        }
       

    }
}