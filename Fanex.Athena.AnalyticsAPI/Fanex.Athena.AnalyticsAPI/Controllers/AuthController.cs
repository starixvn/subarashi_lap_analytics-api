using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Configuration;
using Fanex.Athena.AnalyticsAPI.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;

namespace Fanex.Athena.AnalyticsAPI.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IdentityServerConfig _identityServerConfig;
        private readonly HttpClient _client;

        public AuthController(IOptionsSnapshot<IdentityServerConfig> identityServerConfig, HttpClient client)
        {
            _identityServerConfig = identityServerConfig.Value;
            _client = client;
        }

        [HttpPost]
        [Route("api/auth/token")]
        [AllowAnonymous]
        public async Task<IStatusCodeActionResult> Token([FromBody] LapTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var tokenResponse = await RequestToken(request.Token);
            return Json(new
            {
                IsSuccess = !tokenResponse.IsError,
                Msg = tokenResponse.ErrorDescription,
                AccessToken = tokenResponse.AccessToken
            });
        }

        [HttpGet]
        [Route("Auth/RedirectToLAP/{lapUrl}/{lapVer?}")]
        [AllowAnonymous]
        public ActionResult RedirectToLAP([FromRoute] string lapUrl, [FromRoute] string lapVer = "")
        {
            return Redirect(string.Format("http://{0}/{1}/12bet/Auth/Login?returnUrl=/Dashboard", lapUrl, lapVer));
        }

        private async Task<TokenResponse> RequestToken(string token)
        {
            using var request = new TokenRequest
            {
                Address = $"{_identityServerConfig.Host}/{_identityServerConfig.TokenEndpoint}",
                ClientId = _identityServerConfig.CredentialClient.ClientId,
                ClientSecret = _identityServerConfig.CredentialClient.ClientSecret,
                GrantType = _identityServerConfig.GrantType,
                Parameters = new Parameters(new Dictionary<string, string>
                {
                    { nameof(token), token },
                    { "scope", _identityServerConfig.Scope },
                    { "clien_ip", HttpContext.Connection.RemoteIpAddress.ToString() }
                })
            };

            var tokenResponse = await _client.RequestTokenAsync(request);

            return tokenResponse.Exception != null ? throw tokenResponse.Exception : tokenResponse;
        }
    }
}