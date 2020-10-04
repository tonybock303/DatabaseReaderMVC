using DatabaseReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DatabaseReaderTests.Controllers
{
    public class TestDataObject
    {
        private int numberOfHistoryRecords = 5;
        private int numberOfDayRecords;
        private int maxTotalCalories = 4000;
        private int minTotalCalories = 2500;
        public TestDataObject(int numberOfDays)
        {
            numberOfDayRecords = numberOfDays;
        }
        public List<Day> DayList { get; set; }

        public List<Day> GetRandomDaysList()
        {
            List<Day> randomDayList = new List<Day>();            
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Random r = new Random();
            for (int i = 0; i < numberOfDayRecords; i++)
            {
                Day day = new Day();
                day.Date = today.ToString("dd/MM/yyyy");
                day.IsWorkoutDay = r.Next(10) <= 5;
                day.IsHoliday = r.Next(30) <= 5;                
                randomDayList.Add(day);
                today = today.AddDays(-1);
            }     
            return randomDayList;
        }

        internal List<History> GetRandomHistoryList()
        {
            List<History> randomHistoryList = new List<History>();
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Random r = new Random();
            History history = new History();          

            for (int count = 1; count <= numberOfDayRecords; count++)
            {
                history = new History();
                history.CaloriesBurnt = 0;
                history.Id = currentDate.ToString("dd/MM/yyyy");
                history.timeStamp = currentDate.ToString("dd/MM/yyyy HH:mm:ss");
                randomHistoryList.Add(history);
                int totalCalories = r.Next(minTotalCalories, maxTotalCalories);

                for (int i = 1; i < numberOfHistoryRecords; i++)
                {
                    currentDate = currentDate.AddMinutes(1440 / numberOfHistoryRecords);
                    history = new History();
                    history.CaloriesBurnt = (totalCalories / numberOfHistoryRecords) * i;
                    history.Id = currentDate.ToString("dd/MM/yyyy");
                    history.timeStamp = currentDate.ToString("dd/MM/yyyy HH:mm:ss");
                    randomHistoryList.Add(history);
                }
                history = new History();
                history.CaloriesBurnt = totalCalories;
                history.Id = currentDate.ToString("dd/MM/yyyy");
                history.timeStamp = currentDate.ToString("dd/MM/yyyy") + " 23:59:59";
                randomHistoryList.Add(history);

                currentDate = new DateTime(today.AddDays(-count).Year, today.AddDays(-count).Month, today.AddDays(-count).Day);
            }
            return randomHistoryList;
        }
    }
}
