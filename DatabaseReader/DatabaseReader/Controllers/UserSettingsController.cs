using DatabaseReader.Data;
using DatabaseReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DatabaseReader.Controllers
{
    public class UserSettingsController : Controller
    {
        // GET: Settings
        [HttpGet]
        public ActionResult EditUserSettings()
        {
            IDatabase db = new DataAccess();
            var model = db.GetUserProfile(0);
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserSettings(UserSetting userSetting)
        {
            IDatabase db = new DataAccess();
            db.SaveUserProfile(userSetting);

            return View();
        }
    }
}