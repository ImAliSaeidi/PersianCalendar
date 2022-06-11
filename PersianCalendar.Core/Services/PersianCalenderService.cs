using Newtonsoft.Json;
using PersianCalendar.Core.Convertors;
using PersianCalendar.Core.IServices;
using PersianCalendar.Data.Entities.Enums;
using PersianCalendar.Data.Entities.Reponses;
using PersianCalendar.Data.Entities.Requests;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianCalendar.Core.Services
{
    public class PersianCalendarService : IPersianCalendarService
    {
        private readonly IWebApiClient webApiClient;
        private readonly ITelegramService telegramService;

        public PersianCalendarService(IWebApiClient webApiClient, ITelegramService telegramService)
        {
            this.webApiClient = webApiClient;
            this.telegramService = telegramService;
        }

        private async Task<OccasionsResult> GetShamsiOccasionsOfTheDay(DateOnly date)
        {
            var result = new OccasionsResult();
            var day = DateTime.Now.GetShamsiDay();
            var month = DateTime.Now.GetShamsiMonth();
            var calendarType = CalendarType.Shamsi.GetEnumDescription();
            //var request = new RequestSpecification($"/{CalendarType}/{day}/{month}");
            var request = new RequestSpecification($"/{calendarType}/10/4");

            var response = await webApiClient.Get<RestResponse>(request);
            if (response.Content.Contains("success"))
            {
                result = JsonConvert.DeserializeObject<OccasionsResult>(response.Content);
            }
            return result;
        }

        public async Task<OccasionsResult> GetPersianDate()
        {
            var result = await GetShamsiOccasionsOfTheDay(DateOnly.FromDateTime(DateTime.Now));
            await telegramService.SendDailyOccasions(result);
            return result;
        }
    }
}
