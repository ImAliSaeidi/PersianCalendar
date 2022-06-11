using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianCalendar.Data.Entities.Enums
{
    public enum CalendarType
    {
        [Description("sh")]
        Shamsi,

        [Description("ic")]
        Islamic,

        [Description("wc")]
        World
    }
}
