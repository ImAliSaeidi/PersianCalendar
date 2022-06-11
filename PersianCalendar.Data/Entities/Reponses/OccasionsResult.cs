using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianCalendar.Data.Entities.Reponses
{
    public class OccasionsResult
    {
        public string Type { get; set; }

        public List<DayOccasion> Values { get; set; }
    }
}
