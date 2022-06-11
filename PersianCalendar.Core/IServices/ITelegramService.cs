using PersianCalendar.Data.Entities.Reponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianCalendar.Core.IServices
{
    public interface ITelegramService
    {
        Task SendDailyOccasions(OccasionsResult occasionsResult);

        void Start();
    }
}
