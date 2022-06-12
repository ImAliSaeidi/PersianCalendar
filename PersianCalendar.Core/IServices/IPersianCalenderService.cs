namespace PersianCalendar.Core.IServices
{
    public interface IPersianCalendarService
    {
        string GetPersianDate();

        string GetPersianTime();

        string GetPersianDateTime();

        Task<PrayerTimeResult> GetPrayerTimeForCityOfIran(string cityName);

        Task<OccasionsResult> GetShamsiOccasionsOfDay();
    }
}
