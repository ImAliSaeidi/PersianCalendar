namespace PersianCalendar.Core.Services.HostedService
{
    using Microsoft.Extensions.DependencyInjection;
    using DailyOccasionsTimer = System.Timers.Timer;

    public class DailyOccasionsHostedService : BackgroundService
    {
        private readonly IServiceProvider ServiceProvider;

        private readonly DailyOccasionsTimer dailyOccasionsTimer;


        public DailyOccasionsHostedService(IServiceProvider serviceProvider, IOptions<DailyOccasionsHostedServiceOptions> Options)
        {
            ServiceProvider = serviceProvider;
            dailyOccasionsTimer = new DailyOccasionsTimer(Options.Value.DailyOccasionsInterval.TotalMilliseconds);

            dailyOccasionsTimer.Elapsed += DailyOccasionsTimer_Elapsed;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            dailyOccasionsTimer.Start();
            return Task.CompletedTask;
        }

        private void DailyOccasionsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            dailyOccasionsTimer.Stop();

            using (var serviceScope = ServiceProvider.CreateScope())
            {
                var telegramService = serviceScope.ServiceProvider.GetService<ITelegramService>();
                telegramService.SendDailyOccasions();
            }

            dailyOccasionsTimer.Start();
        }
    }
}
