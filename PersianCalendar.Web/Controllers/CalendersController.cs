namespace PersianCalendar.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalendersController : ControllerBase
    {
        private readonly ICalendarService CalendarService;

        public CalendersController(ICalendarService CalendarService)
        {
            this.CalendarService = CalendarService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDate()
        {
            return Ok(await CalendarService.GetPrayerTimeForCityOfIran("تهران"));
        }
    }
}