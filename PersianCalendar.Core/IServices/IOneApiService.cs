namespace PersianCalendar.Core.IServices
{
    public interface IOneApiService
    {
        Task<string> GetPrayerTimeForCityOfIran(string cityName);

        Task<string> GetHafezOmen();
    }
}
