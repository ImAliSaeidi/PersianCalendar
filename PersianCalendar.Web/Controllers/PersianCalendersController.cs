using Microsoft.AspNetCore.Mvc;
using PersianCalendar.Core.IServices;

namespace PersianCalendar.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersianCalendarsController : ControllerBase
    {
        private readonly IPersianCalendarService persianCalendarService;

        public PersianCalendarsController(IPersianCalendarService persianCalendarService)
        {
            this.persianCalendarService = persianCalendarService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDate()
        {
            return Ok(await persianCalendarService.GetPersianDate());
        }
    }
}