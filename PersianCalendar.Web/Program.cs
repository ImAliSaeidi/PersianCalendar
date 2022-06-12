
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICalendarWebApiClient, CalendarWebApiClient>();
builder.Services.AddScoped<IOneApiWebApiClient, OneApiWebApiClient>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IOneApiService, OneApiService>();
builder.Services.AddScoped<ITelegramService, TelegramService>();

builder.Configuration.GetSection("CalendarAPIConfiguration").Bind(new CalendarAPIConfiguration());
builder.Configuration.GetSection("TelegramBotConfig").Bind(new TelegramBotConfig());
builder.Configuration.GetSection("OneApiServiceConfig").Bind(new OneApiServiceConfig());
builder.Services.Configure<MongoDBConfig>(builder.Configuration.GetSection("MongoDBConfig"));
builder.Services.AddSingleton<MongoService>();

var telegramService = builder.Services.BuildServiceProvider().GetRequiredService<ITelegramService>();
telegramService.Start();
await telegramService.SendStartMessage();
await telegramService.SendDailyOccasions();

builder.Services.AddHostedService<DailyOccasionsHostedService>();

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
