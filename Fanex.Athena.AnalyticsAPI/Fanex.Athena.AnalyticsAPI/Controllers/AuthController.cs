using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Controllers
{
    [ApiController]
    public class AuthController : ControllerBaseCustom
    {
        private readonly IAuthService _authService;
        private const string AUTHKEY = "X-Auth-Token";

        public AuthController(IContextService contextService, IAuthService authService)
            : base(contextService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("auth/token")]
        public async Task<JsonResult> GetToken()
        {
            if (HttpContext.Request.Headers.TryGetValue(AUTHKEY, out StringValues lapToken))
            {
                var jwt = await _authService.Authenticate(lapToken[0]);
                if (string.IsNullOrEmpty(jwt))
                {
                    _contextService.AddError("token is invalid");
                }
                return ToResult(new { token = jwt });
            }
            _contextService.AddError("token is invalid");
            return ToResult();
        }
    }
}