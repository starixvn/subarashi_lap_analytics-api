using Fanex.Athena.AnalyticsAPI.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fanex.Athena.AnalyticsAPI.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private const int ThirtyMinutes = 60 * 30;

        [HttpGet]
        [Route("api/static")]
        [AllowAnonymous]
        [ResponseCache(Duration = ThirtyMinutes, Location = ResponseCacheLocation.Client)]
        public StaticResourceConfig Static([FromServices] IOptions<StaticResourceConfig> config)
        {
            return config.Value;
        }
    }
}