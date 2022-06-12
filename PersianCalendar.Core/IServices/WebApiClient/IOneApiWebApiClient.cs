namespace PersianCalendar.Core.IServices.WebApiClient
{
    public interface IOneApiWebApiClient
    {
        Task<RestResponse<T>> Get<T>(RequestSpecification requestSpecification);
    }
}
