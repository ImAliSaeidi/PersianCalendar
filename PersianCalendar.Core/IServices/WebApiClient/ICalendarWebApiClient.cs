namespace PersianCalendar.Core.IServices.WebApiClient
{
    public interface ICalendarWebApiClient
    {
        Task<RestResponse<T>> Get<T>(RequestSpecification requestSpecification);
    }
}
