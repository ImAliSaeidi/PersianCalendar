using PersianCalendar.Core.Convertors;
using PersianCalendar.Core.IServices;
using PersianCalendar.Data.Entities.Reponses;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PersianCalendar.Core.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly TelegramBotClient botClient;
        private const string ChatId = "395886871";

        public TelegramService()
        {
            botClient = new TelegramBotClient("5414851703:AAHtzWoRTEv9ak_kFFeyPfpRYe0xm3dSdqU");
        }

        public void Start()
        {
            using var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                errorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
                return;

            if (update.Message!.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "You said:\n" + messageText,
                cancellationToken: cancellationToken);
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public async Task SendDailyOccasions(OccasionsResult occasionsResult)
        {
            var occasionsList = occasionsResult.Values.Select(x => x.Occasion).ToList();
            var occasionsString = "";
            for (int i = 0; i < occasionsList.Count; i++)
            {
                occasionsString += $"{i + 1}){occasionsList[i]}\n";
            }
            var message = $"تاریخ میلادی:\n{DateTime.Now.ToShortDateString()}\n" +
                $"{new string('-', 15)}\n" +
                $"تاریخ شمسی:\n{DateTime.Now.ToShamsiDateOnly()}\n" +
                $"{new string('-', 15)}\n" +
                $"مناسبت های روز:\n{occasionsString}";
            await botClient.SendTextMessageAsync(ChatId, message);
        }
    }
}
