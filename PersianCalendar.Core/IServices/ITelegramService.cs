namespace PersianCalendar.Core.IServices
{
    public interface ITelegramService
    {
        Task SendDailyOccasions(long chatId, OccasionsResult occasionsResult);

        Task ResponseToCommand(long chatId, string messageText);

        void Start();
    }
}
