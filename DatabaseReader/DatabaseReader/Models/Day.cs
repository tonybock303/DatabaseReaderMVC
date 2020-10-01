//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseReader.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Day
    {
        DateHelper dateHelper = new DateHelper();
        public string Date { get; set; }
        public DateTime DateTime
        {
            get
            {
                return Date != null ? DateTime.Parse(Date) : new DateTime() ;
            }
        }
        public string DateTimeString
        {
            get
            {
                return DateTime.ToString("ddd dd/MM/yyyy");
            }
        }
        public bool IsWorkoutDay { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsDefault { get; set; }
        public int TotalCalories { get; set; }
        public bool IsSameDate(DateTime date) => dateHelper.IsSameDate(DateTime, date);
    }
}