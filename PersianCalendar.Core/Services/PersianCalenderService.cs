namespace PersianCalendar.Core.Services
{
    public class PersianCalendarService : IPersianCalendarService
    {
        private readonly IPersianCalendarWebApiClient persianCalendarWebApiClient;
        private IPrayerTimeWebApiClient pryerTimeWebApiClient;

        public PersianCalendarService(IPersianCalendarWebApiClient persianCalendarWebApiClient, IPrayerTimeWebApiClient pryerTimeWebApiClient)
        {
            this.persianCalendarWebApiClient = persianCalendarWebApiClient;
            this.pryerTimeWebApiClient = pryerTimeWebApiClient;
        }

        public async Task<OccasionsResult> GetShamsiOccasionsOfDay()
        {
            var result = new OccasionsResult();
            var day = DateTime.Now.GetShamsiDay();
            var month = DateTime.Now.GetShamsiMonth();
            var calendarType = CalendarType.Shamsi.GetEnumDescription();
            var request = new RequestSpecification($"/{calendarType}/{day}/{month}");

            var response = await persianCalendarWebApiClient.Get<RestResponse>(request);
            if (response.Content.Contains("success"))
            {
                result = JsonConvert.DeserializeObject<OccasionsResult>(response.Content);
            }
            return result;
        }

        public string GetPersianDate()
        {
            return DateTime.Now.ToShamsiDateOnly();
        }

        public string GetPersianTime()
        {
            return DateTime.Now.ToLongTimeString();
        }

        public string GetPersianDateTime()
        {
            return DateTime.Now.ToShamsiDateTime();
        }

        public async Task<PrayerTimeResult> GetPrayerTimeForCityOfIran(string cityName)
        {
            var result = new PrayerTimeResult();
            var request = new RequestSpecification(string.Empty);
            request.QueryParameters.Add("token", PrayerTimeApiConfig.Token);
            request.QueryParameters.Add("city", cityName);
            request.QueryParameters.Add("en_num", "true");

            var response = await pryerTimeWebApiClient.Get<RestResponse>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = JsonConvert.DeserializeObject<PrayerTimeResult>(response.Content);
            }

            return result;
        }
    }
}
