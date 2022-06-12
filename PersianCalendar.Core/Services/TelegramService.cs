namespace PersianCalendar.Core.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly TelegramBotClient botClient;
        private readonly IPersianCalendarService persianCalendarService;
        private string LastCommand;
        private long ChatId;

        public TelegramService(IPersianCalendarService persianCalendarService)
        {
            botClient = new TelegramBotClient(TelegramBotConfig.Token);
            this.persianCalendarService = persianCalendarService;
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
                await SendPrayerTimeMessage(chatId, command.Replace("/", ""));
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

        private async Task SendDailyOccasions(long chatId, OccasionsResult occasionsResult)
        {
            var occasionsList = occasionsResult.Values.Select(x => x.Occasion).ToList();
            var occasionsString = "مناسبتی وجود ندارد";
            if (occasionsList.Count != 0)
            {
                occasionsString = "";
                for (int i = 0; i < occasionsList.Count; i++)
                {
                    occasionsString += $"{i + 1}){occasionsList[i]}\n";
                }
            }

            var message = $"تاریخ میلادی:\n{DateTime.Now.ToShortDateString()}\n" +
                $"{new string('-', 15)}\n" +
                $"تاریخ شمسی:\n{DateTime.Now.ToShamsiDateOnly()}\n" +
                $"{new string('-', 15)}\n" +
                $"مناسبت های روز:\n{occasionsString}";
            await botClient.SendTextMessageAsync(chatId, message);
        }

        private async Task ResponseToCommand(long chatId, string command)
        {
            LastCommand = command;
            ChatId = chatId;
            switch (command)
            {
                case "/occasions":
                    await SendDailyOccasions(chatId, await persianCalendarService.GetShamsiOccasionsOfDay());
                    break;
                case "/time":
                    await SendMessage(chatId, persianCalendarService.GetPersianTime());
                    break;
                case "/date":
                    await SendMessage(chatId, persianCalendarService.GetPersianDate());
                    break;
                case "/datetime":
                    await SendMessage(chatId, persianCalendarService.GetPersianDateTime());
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
            await botClient.SendTextMessageAsync(chatId, message);
        }

        private async Task SendPrayerTimeMessage(long chatId, string cityName)
        {
            var prayerTimeResult = await persianCalendarService.GetPrayerTimeForCityOfIran(cityName);
            await botClient.SendTextMessageAsync(chatId, prayerTimeResult.Result.ToString());
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
