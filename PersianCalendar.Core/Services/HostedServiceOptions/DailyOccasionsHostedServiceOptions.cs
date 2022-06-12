namespace PersianCalendar.Core.Services.HostedServiceOptions
{
    public class DailyOccasionsHostedServiceOptions
    {
        public TimeSpan DailyOccasionsInterval { get; set; } = TimeSpan.FromDays(1);
    }
}
