namespace PersianCalendar.Core.Services.WebApiClient
{
    public class CalendarWebApiClient : ICalendarWebApiClient
    {
        public async Task<RestResponse<T>> Get<T>(RequestSpecification requestSpecification)
        {
            return await SendRequest<T>(requestSpecification, Method.Get);
        }

        public async Task<RestResponse<T>> Post<T>(RequestSpecification requestSpecification)
        {
            return await SendRequest<T>(requestSpecification, Method.Post);
        }

        public async Task<RestResponse<T>> Delete<T>(RequestSpecification requestSpecification)
        {
            return await SendRequest<T>(requestSpecification, Method.Delete);
        }
        private async Task<RestResponse<T>> SendRequest<T>(RequestSpecification requestSpecification, Method method)
        {
            var client = CreateClient(requestSpecification);
            var request = CreateRequest(method, requestSpecification);
            return await client.ExecuteAsync<T>(request);
        }

        private static RestClient CreateClient(RequestSpecification requestSpecification)
        {
            return new RestClient(CalendarAPIConfiguration.Route + requestSpecification.Endpoint);
        }

        private static RestRequest CreateRequest(Method method, RequestSpecification requestSpecification)
        {
            var request = new RestRequest();
            request.Method = method;
            request.AddHeader("Content-Type", "application/json");
            return request;
        }



    }
}
