using Microsoft.AspNetCore.Mvc;

namespace InventrySystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Backend is connected successfully!", timestamp = DateTime.UtcNow });
        }
    }
} 