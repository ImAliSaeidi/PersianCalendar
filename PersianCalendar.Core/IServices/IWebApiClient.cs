using PersianCalendar.Data.Entities.Requests;
using RestSharp;

namespace PersianCalendar.Core.IServices
{
    public interface IWebApiClient
    {
        Task<RestResponse<T>> Get<T>(RequestSpecification requestSpecification);
        Task<RestResponse<T>> Post<T>(RequestSpecification requestSpecification);
        Task<RestResponse<T>> Delete<T>(RequestSpecification requestSpecification);
    }
}
