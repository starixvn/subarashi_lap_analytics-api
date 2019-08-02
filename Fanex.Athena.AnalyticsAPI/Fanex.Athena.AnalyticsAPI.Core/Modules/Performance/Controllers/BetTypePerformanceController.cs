using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetTypeController : ControllerCustomBase
    {
        public BetTypeController()
        {

        }

        [HttpGet]
        public IActionResult Get()
        {
            return ToResult(new { a = "abcxyz" });
        }
    }
}