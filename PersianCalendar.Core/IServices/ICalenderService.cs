namespace PersianCalendar.Core.IServices
{
    public interface ICalendarService
    {
        string GetDate();

        string GetTime();

        string GetPersianDateTime();

        Task<string> GetPrayerTimeForCityOfIran(string cityName);

        Task<string> GetOccasionsOfDay();
    }
}
