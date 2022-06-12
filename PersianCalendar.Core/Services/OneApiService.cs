namespace PersianCalendar.Core.Services
{
    public class OneApiService : IOneApiService
    {
        private readonly IOneApiWebApiClient oneApiWebApiClient;

        public OneApiService(IOneApiWebApiClient oneApiWebApiClient)
        {
            this.oneApiWebApiClient = oneApiWebApiClient;
        }

        public async Task<string> GetHafezOmen()
        {
            var result = "";
            var oneApiResult = new OneApiResult<HafezOmen>();

            var request = new RequestSpecification(OneApiEndpoints.Omen);

            var response = await oneApiWebApiClient.Get<RestRequest>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                oneApiResult = JsonConvert.DeserializeObject<OneApiResult<HafezOmen>>(response.Content);
                result = oneApiResult.Result.ToString();
            }

            return result;
        }

        public async Task<string> GetPrayerTimeForCityOfIran(string cityName)
        {
            var result = "متاسفانه شهر مورد نظر یافت نشد";
            var prayerTimeResult = new OneApiResult<PrayerTime>();
            var request = new RequestSpecification(OneApiEndpoints.Owghat);

            request.QueryParameters.Add("city", cityName);
            request.QueryParameters.Add("en_num", "true");

            var response = await oneApiWebApiClient.Get<RestResponse>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                prayerTimeResult = JsonConvert.DeserializeObject<OneApiResult<PrayerTime>>(response.Content);
                if (prayerTimeResult.Result != null)
                {
                    result = prayerTimeResult.Result.ToString();
                }
            }

            return result;
        }
    }
}
