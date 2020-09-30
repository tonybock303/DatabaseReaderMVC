using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseReader.Models
{
    public class DateHelper
    {
        public bool IsSameDate(DateTime date1, DateTime date2) => (date1.Day == date2.Day && date1.Month == date2.Month && date1.Year == date2.Year);

        public bool IsTheSameDayOfTheWeek(DateTime date1, DateTime date2) => (date1.DayOfWeek == date2.DayOfWeek);

        public bool IsWeekDay(DateTime date) => (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday);

        public bool IsWeekEnd(DateTime date) => (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);

        public bool IsSameTime(DateTime date1, DateTime date2) => (date1.Hour == date2.Hour && date1.Minute == date2.Minute);
    }
}