using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseReader.Models
{
    public class DayModel
    {

        public Day Day { get; set; }
        public List<History> History { get; set; }
        public string id { get; set; }
    }
}