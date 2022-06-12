namespace PersianCalendar.Core.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly ICalendarWebApiClient calendarWebApiClient;
        private IOneApiWebApiClient pryerTimeWebApiClient;

        public CalendarService(ICalendarWebApiClient calendarWebApiClient, IOneApiWebApiClient pryerTimeWebApiClient)
        {
            this.calendarWebApiClient = calendarWebApiClient;
            this.pryerTimeWebApiClient = pryerTimeWebApiClient;
        }

        private static string FillShamsiOccasionsOfDay(List<DayOccasion> shamsiOccasionsResult)
        {
            var result = "";

            if (shamsiOccasionsResult.Count != 0)
            {
                result += "تقویم شمسی:\n";

                for (int i = 0; i < shamsiOccasionsResult.Count; i++)
                {
                    result += $"{i + 1}){shamsiOccasionsResult[i].ToString()}";
                }

                result += $"{new string('-', 50)}\n";
            }

            return result;
        }
        private static string FillGregorianOccasionsOfDay(List<DayOccasion> gregorianOccasionsResult)
        {
            var result = "";

            if (gregorianOccasionsResult.Count != 0)
            {
                result += "تقویم میلادی:\n";

                for (int i = 0; i < gregorianOccasionsResult.Count; i++)
                {
                    result += $"{i + 1}){gregorianOccasionsResult[i].ToString()}";
                }

                result += $"{new string('-', 50)}\n";
            }

            return result;
        }

        private static string FillHijriOccasionsOfDay(List<DayOccasion> hijriOccasionsResult)
        {
            var result = "";

            if (hijriOccasionsResult.Count != 0)
            {
                result += "تقویم قمری:\n";

                for (int i = 0; i < hijriOccasionsResult.Count; i++)
                {
                    result += $"{i + 1}){hijriOccasionsResult[i].ToString()}";
                }
            }

            return result;
        }

        public async Task<string> GetOccasionsOfDay()
        {
            var result = "";

            var occasionsResult = new OccasionsResult();

            var shamsiDate = DateTime.Now.ToShamsiDateOnly();
            var gregorianDate = DateTime.Now.ToGregorianDateOnly();
            var hijriDate = DateTime.Now.ToHijriDateOnly();

            var persianDayofMonth = DateTime.Now.GetShamsiDayOfMonth();
            var persianMonth = DateTime.Now.GetShamsiMonth();

            var gregorianDayOfMonth = DateTime.Now.GetGregorianDayOfMonth();
            var gregorianMonth = DateTime.Now.GetGregorianMonth();

            var hijriDayOfMonth = DateTime.Now.GetHijriDayOfMonth();
            var hijriMonth = DateTime.Now.GetHijriMonth();

            var endpoint = $"/sh,wc,ic/" +
                $"{persianDayofMonth}," +
                $"{gregorianDayOfMonth}," +
                $"{hijriDayOfMonth}/" +
                $"{persianMonth}," +
                $"{gregorianMonth}," +
                $"{hijriMonth}";

            var request = new RequestSpecification(endpoint);

            var response = await calendarWebApiClient.Get<RestResponse>(request);
            if (response.Content.Contains("success"))
            {
                occasionsResult = JsonConvert.DeserializeObject<OccasionsResult>(response.Content);

                result +=
                  $"تاریخ شمسی : {shamsiDate}\n" +
                  $"{new string('-', 50)}\n" +
                  $"تاریخ میلادی : {gregorianDate}\n" +
                  $"{new string('-', 50)}\n" +
                  $"تاریخ قمری : {hijriDate}\n" +
                  $"{new string('-', 100)}\n" +
                  $"مناسبت های امروز:\n";

                if (occasionsResult.Values.Count != 0)
                {
                    result += FillShamsiOccasionsOfDay(occasionsResult.Values.Where(x => x.Type == "SH").ToList());
                    result += FillGregorianOccasionsOfDay(occasionsResult.Values.Where(x => x.Type == "WC").ToList());
                    result += FillHijriOccasionsOfDay(occasionsResult.Values.Where(x => x.Type == "IC").ToList());
                }
                else
                {
                    result += "مناسبتی وجود ندارد";
                }
            }

            return result;
        }

        public string GetDate()
        {
            return
                $"تاریخ شمسی : {DateTime.Now.ToShamsiDateOnly()}\n" +
                $"تاریخ میلادی : {DateTime.Now.ToGregorianDateOnly()}\n" +
                $"تاریخ قمری : {DateTime.Now.ToHijriDateOnly()}";
        }

        public string GetTime()
        {
            return DateTime.Now.ToLongTimeString();
        }

        public string GetDateTime()
        {
            return DateTime.Now.ToShamsiDateTime();
        }

    }
}
