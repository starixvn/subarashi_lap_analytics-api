using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerCustomBase
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return ToResult(new { Name = "" });
        }
    }
}