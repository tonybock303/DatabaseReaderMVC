using DatabaseReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseReader.Data
{
    public interface IDatabase
    {
        List<Day> GetAllDays();
        List<History> GetAllHistories();

        UserSetting GetUserProfile(int id);
        void SaveUserProfile(UserSetting userSetting);
    }
}
