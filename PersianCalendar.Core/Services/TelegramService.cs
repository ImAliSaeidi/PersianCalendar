﻿namespace PersianCalendar.Core.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly TelegramBotClient botClient;
        private readonly ICalendarService CalendarService;
        private readonly IOneApiService oneApiService;
        private string LastCommand;
        private long ChatId;

        public TelegramService(ICalendarService CalendarService, IOneApiService oneApiService)
        {
            botClient = new TelegramBotClient(TelegramBotConfig.Token);
            this.CalendarService = CalendarService;
            this.oneApiService = oneApiService;
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
            var command = update.Message.Text;
            if (command.Contains('@'))
            {
                command = command.Split('@')[0];
            }
            if (chatId == ChatId && LastCommand == "/prayertimes")
            {
                await SendMessage(chatId, await oneApiService.GetPrayerTimeForCityOfIran(command.Replace("/", "")));
            }
            await ResponseToCommand(chatId, command);
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

        private async Task ResponseToCommand(long chatId, string command)
        {
            LastCommand = command;
            ChatId = chatId;
            switch (command)
            {
                case "/occasions":
                    await SendMessage(chatId, await CalendarService.GetOccasionsOfDay());
                    break;
                case "/omen":
                    await SendMessage(chatId, await oneApiService.GetHafezOmen());
                    break;
                case "/date":
                    await SendMessage(chatId, CalendarService.GetDate());
                    break;
                case "/time":
                    await SendMessage(chatId, CalendarService.GetTime());
                    break;
                case "/datetime":
                    await SendMessage(chatId, CalendarService.GetDateTime());
                    break;
                case "/prayertimes":
                    await SendChooseCityMessage(chatId);
                    break;
                default:
                    break;
            }
        }

        private async Task SendMessage(long chatId, string message)
        {
            if (message.Length > 4096)
            {
                message = "متاسفانه خطایی رخ داده است،لطفا کمی بعد دوباره تلاش کنید";
            }
            await botClient.SendTextMessageAsync(chatId, message);
        }

        private async Task SendChooseCityMessage(long chatId)
        {
            var message = "لطفا نام شهر مورد نظر خود را با فرمت زیر ارسال کنید:\n/تهران";
            await botClient.SendTextMessageAsync(chatId, message);
        }

        public async Task SendStartMessage()
        {
            await botClient.SendTextMessageAsync("395886871", "Start");
        }
    }
}
