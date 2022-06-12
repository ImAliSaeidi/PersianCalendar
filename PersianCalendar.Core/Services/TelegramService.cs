namespace PersianCalendar.Core.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly TelegramBotClient botClient;
        private readonly ICalendarService calendarService;
        private readonly IOneApiService oneApiService;
        private readonly MongoService mongoService;
        private string LastCommand;
        private long ChatId;

        public TelegramService(ICalendarService calendarService, IOneApiService oneApiService, MongoService mongoService)
        {
            botClient = new TelegramBotClient(TelegramBotConfig.Token);
            this.calendarService = calendarService;
            this.oneApiService = oneApiService;
            this.mongoService = mongoService;
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
            var messageId = update.Message.MessageId;

            await mongoService.SaveUserData(new UserInfo() { UserId = chatId });

            if (command.Contains('@'))
            {
                command = command.Split('@')[0];
            }
            if (chatId == ChatId && LastCommand == "/prayertimes")
            {
                await SendMessage(chatId, await oneApiService.GetPrayerTimeForCityOfIran(command.Replace("/", "")), messageId);
            }
            await ResponseToCommand(chatId, command, messageId);
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

        private async Task ResponseToCommand(long chatId, string command, int messageId)
        {
            LastCommand = command;
            ChatId = chatId;
            switch (command)
            {
                case "/occasions":
                    await SendMessage(chatId, await calendarService.GetOccasionsOfDay(), messageId);
                    break;
                case "/omen":
                    await SendMessage(chatId, await oneApiService.GetHafezOmen(), messageId);
                    break;
                case "/date":
                    await SendMessage(chatId, calendarService.GetDate(), messageId);
                    break;
                case "/time":
                    await SendMessage(chatId, calendarService.GetTime(), messageId);
                    break;
                case "/datetime":
                    await SendMessage(chatId, calendarService.GetDateTime(), messageId);
                    break;
                case "/prayertimes":
                    await SendChooseCityMessage(chatId);
                    break;
                default:
                    break;
            }
        }

        private async Task SendMessage(long chatId, string message, int? messageId)
        {
            if (message.Length > 4096)
            {
                message = "متاسفانه خطایی رخ داده است،لطفا کمی بعد دوباره تلاش کنید";
            }
            await botClient.SendTextMessageAsync(chatId: chatId, text: message, replyToMessageId: messageId);
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

        public async Task SendDailyOccasions()
        {
            var message = await calendarService.GetOccasionsOfDay();
            var users = await mongoService.GetAsync();
            foreach (var user in users)
            {
                await SendMessage(user.UserId, message, null);
            }
        }
    }
}
