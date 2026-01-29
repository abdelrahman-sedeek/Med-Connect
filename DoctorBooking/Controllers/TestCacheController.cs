using DoctorBooking.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DoctorBooking.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestCacheController : ControllerBase
    {
        [Caching]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("every things okay");
        }
    }
}
