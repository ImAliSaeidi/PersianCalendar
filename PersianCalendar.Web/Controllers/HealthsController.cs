namespace PersianCalendar.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthsController : ControllerBase
    {

        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok();
        }
    }
}