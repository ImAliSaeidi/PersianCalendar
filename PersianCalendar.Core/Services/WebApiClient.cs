using PersianCalendar.Core.IServices;
using PersianCalendar.Data.Entities.Configs;
using PersianCalendar.Data.Entities.Requests;
using RestSharp;

namespace PersianCalendar.Core.Services
{
    public class WebApiClient : IWebApiClient
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
            requestSpecification.Endpoint = AddQueryParameters(request, requestSpecification);
            AddJSONBody(request, requestSpecification.Body);
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

        private static string AddQueryParameters(RestRequest request, RequestSpecification requestSpecification)
        {
            if (requestSpecification.QueryParameters.Count != 0)
            {
                requestSpecification.Endpoint += "?";
                foreach (var item in requestSpecification.QueryParameters)
                {
                    requestSpecification.Endpoint += item.Key + "=" + item.Value + "&";
                    request.AddQueryParameter(item.Key, item.Value);
                }

                requestSpecification.Endpoint = requestSpecification.Endpoint.Substring(0, requestSpecification.Endpoint.Length - 1);
            }
            return requestSpecification.Endpoint;
        }

        private static void AddJSONBody(RestRequest request, string body)
        {
            if (body != null)
            {
                request.RequestFormat = DataFormat.Json;
                request.AddBody(body, "application/json");
            }

        }


    }
}
