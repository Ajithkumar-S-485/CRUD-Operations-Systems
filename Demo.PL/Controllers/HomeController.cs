using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Demo API is running successfully!");
        }

        [HttpGet("health")]
        public ActionResult<object> Health()
        {
            return Ok(new { 
                status = "healthy", 
                timestamp = DateTime.UtcNow,
                version = "1.0.0"
            });
        }
    }
}
