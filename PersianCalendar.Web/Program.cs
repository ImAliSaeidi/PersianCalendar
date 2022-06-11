using PersianCalendar.Core.IServices;
using PersianCalendar.Core.Services;
using PersianCalendar.Data.Entities.Configs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IWebApiClient, WebApiClient>();
builder.Services.AddScoped<IPersianCalendarService, PersianCalendarService>();
builder.Services.AddScoped<ITelegramService, TelegramService>();
builder.Configuration.GetSection("CalendarAPIConfiguration").Bind(new CalendarAPIConfiguration());

builder.Services.BuildServiceProvider().GetRequiredService<ITelegramService>().Start();

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
