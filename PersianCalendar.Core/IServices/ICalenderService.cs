namespace PersianCalendar.Core.IServices
{
    public interface ICalendarService
    {
        string GetDate();

        string GetTime();

        string GetDateTime();

        Task<string> GetOccasionsOfDay();
    }
}
