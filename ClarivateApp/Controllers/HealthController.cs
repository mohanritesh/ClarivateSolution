using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClarivateApp.Controllers
{
    
    [ApiController]
    public class HealthController : ControllerBase
    {

        /// <summary>
        /// Health Controller
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("healthCheck")]
        public IActionResult Index()
        {
            return Ok("Api is running");
        }
    }

    
}
