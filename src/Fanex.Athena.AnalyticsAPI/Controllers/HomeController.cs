namespace Fanex.Athena.AnalyticsAPI.Controllers
{
    using Configuration;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("api/static")]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, NoStore = true, Location = ResponseCacheLocation.None)]
        public StaticResourceConfig Static([FromServices] IOptions<StaticResourceConfig> config)
            => config.Value;
    }
}