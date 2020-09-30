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
        [Required]
        [System.ComponentModel.DataAnnotations.Timestamp]
        public DateTime StartTime { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.DataType(DataType.Duration)]
        public int Mins { get; set; }
        
        [Required]
        public int Burnt { get; set; }
    }
}