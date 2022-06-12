
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPersianCalendarWebApiClient, PersianCalendarWebApiClient>();
builder.Services.AddScoped<IPrayerTimeWebApiClient, PrayerTimeWebApiClient>();
builder.Services.AddScoped<IPersianCalendarService, PersianCalendarService>();
builder.Services.AddScoped<ITelegramService, TelegramService>();
builder.Configuration.GetSection("CalendarAPIConfiguration").Bind(new CalendarAPIConfiguration());
builder.Configuration.GetSection("TelegramBotConfig").Bind(new TelegramBotConfig());
builder.Configuration.GetSection("PrayerTimeApiConfig").Bind(new PrayerTimeApiConfig());

var telegramService = builder.Services.BuildServiceProvider().GetRequiredService<ITelegramService>();
telegramService.Start();
telegramService.SendStartMessage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
