using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBaseCustom
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return ToResult(new { Name = "" });
        }
    }
}