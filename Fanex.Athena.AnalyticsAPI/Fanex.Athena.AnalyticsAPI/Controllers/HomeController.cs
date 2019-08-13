using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBaseCustom
    {
        private readonly IHostingEnvironment _env;
        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            
            return ToResult(new { Name = "", _env.EnvironmentName });
        }
    }
}