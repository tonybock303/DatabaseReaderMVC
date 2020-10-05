using DatabaseReader.Data;
using DatabaseReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseReaderTests.Controllers
{
    public class TestDataAccess : IDatabase
    {
        private TestDataObject testDataObject = new TestDataObject(100);
        public List<Day> GetAllDays()
        {
            return testDataObject.GetRandomDaysList();
        }

        public List<History> GetAllHistories()
        {
            return testDataObject.GetRandomHistoryList();
        }

        public UserSetting GetUserProfile(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveUserProfile(UserSetting userSetting)
        {
            throw new NotImplementedException();
        }
    }
}
