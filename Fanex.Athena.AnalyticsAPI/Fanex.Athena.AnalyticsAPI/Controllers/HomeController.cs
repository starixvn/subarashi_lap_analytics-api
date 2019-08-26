using Fanex.Athena.AnalyticsAPI.Core.Controllers;
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

        [HttpGet]
        public IActionResult Get()
        {
            return ToResult(new { Name = "", _env.EnvironmentName });
        }
    }
}