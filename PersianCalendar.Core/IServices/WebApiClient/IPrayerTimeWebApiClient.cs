namespace PersianCalendar.Core.IServices.WebApiClient
{
    public interface IPrayerTimeWebApiClient
    {
        Task<RestResponse<T>> Get<T>(RequestSpecification requestSpecification);
        Task<RestResponse<T>> Post<T>(RequestSpecification requestSpecification);
        Task<RestResponse<T>> Delete<T>(RequestSpecification requestSpecification);
    }
}
