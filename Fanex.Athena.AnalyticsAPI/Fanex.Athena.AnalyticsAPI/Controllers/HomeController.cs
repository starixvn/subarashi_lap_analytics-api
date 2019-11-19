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

        public HomeController(IHostingEnvironment env) : base(null)
        {
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return ToResult(new { Name = "", _env.EnvironmentName });
        }
    }
}