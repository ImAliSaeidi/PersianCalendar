using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianCalendar.Data.Entities.Reponses
{
    public class DayOccasion
    {
        public int Id { get; set; }

        public int? Year { get; set; }

        public bool DayOff { get; set; }

        public string Type { get; set; }

        public string Category { get; set; }


        public string Occasion { get; set; }
    }
}
