using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace DatabaseReader.Models
{
    public class WorkoutModel
    {
        [Required(ErrorMessage = "Please enter the start time of your workout")]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Please enter the number of minutes you worked out")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum workout time is 1 minute")]
        public int Mins { get; set; }

        [Required(ErrorMessage = "Please enter the number of calories you burnt")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum calories burnt is 1 calorie")]
        public int Burnt { get; set; }
    }
}