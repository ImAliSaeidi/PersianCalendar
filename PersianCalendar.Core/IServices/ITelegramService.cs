namespace PersianCalendar.Core.IServices
{
    public interface ITelegramService
    {
        void Start();

        Task SendStartMessage();
    }
}
