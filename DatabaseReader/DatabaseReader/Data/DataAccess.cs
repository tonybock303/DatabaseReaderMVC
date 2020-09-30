using DatabaseReader.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace DatabaseReader.Data
{
    public class DataAccess : IDatabase
    {
        public List<Day> GetAllDays()
        {
            DaysDatabaseEntities db = new DaysDatabaseEntities();
            List<Day> days = db.Days.ToList();
            return days;
        }

        public List<History> GetAllHistories()
        {
            HistoryDatabaseEntitiesNEW db = new HistoryDatabaseEntitiesNEW();
            List<History> days = db.Histories.ToList();
            return days;
        }

        public UserSetting GetUserProfile(int id)
        {
            UserSettingsDatabaseEntities db = new UserSettingsDatabaseEntities();
            UserSetting up = db.UserSettings.FirstOrDefault(i => i.Id == id);
            return up;
        }

        public void SaveUserProfile(UserSetting userSetting)
        {
            UserSettingsDatabaseEntities db = new UserSettingsDatabaseEntities();
            db.UserSettings.AddOrUpdate(i => i.Id == userSetting.Id, userSetting);
            db.SaveChanges();
        }
    }
}