using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.Models;
using Fanex.Athena.AnalyticsAPI.Core.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using LAP.ONEBook.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Handlers
{
    public class ONEBookAuthenticationHandler : AuthenticationHandler<ONEBookAuthenticationOptions>
    {
        private readonly IAuth<DefaultAuthInfo<Permission, CurrencyAgentInfo>> _authService;

        public ONEBookAuthenticationHandler(IOptionsMonitor<ONEBookAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            IAuth<DefaultAuthInfo<Permission, CurrencyAgentInfo>> authService) : base(options, logger, encoder, clock)
        {
            _authService = authService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var existAuthToken = TryGetToken(Request, out string token);
            if (existAuthToken)
            {
                var authInfo = await _authService.AuthenticateAsync(token).ConfigureAwait(false);
                if (ValidateToken(authInfo, Request.Path))
                {
                    var roles = authInfo.IsAdminstrator ? new[] { PermissionConst.SUPER_ADMIN } :
                        authInfo.Permissions.Select(_ => _.FunctionKey).ToArray();

                    var identityClaim = new Claim(ClaimTypes.Name, authInfo.SiteName);
                    var userDataClaim = new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(authInfo));
                    var claims = new ClaimsIdentity(new[] { identityClaim, userDataClaim }, Scheme.Name);
                    var principal = new GenericPrincipal(claims, roles);

                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
            }
            return AuthenticateResult.NoResult();
        }

        internal static bool TryGetToken(HttpRequest request, out string token)
        {
            if (request.Query.TryGetValue(AuthConst.AUTH_KEY, out StringValues queryToken))
            {
                token = queryToken.FirstOrDefault();
            }
            else if (request.Headers.TryGetValue(AuthConst.AUTH_KEY, out StringValues headerToken))
            {
                token = headerToken.FirstOrDefault();
            }
            else
            {
                token = null;
            }

            return !string.IsNullOrEmpty(token);
        }

        internal static bool ValidateToken(DefaultAuthInfo<Permission, CurrencyAgentInfo> authInfo, string actionLink)
        {
            if (string.IsNullOrEmpty(authInfo?.SiteName) || string.IsNullOrEmpty(authInfo?.UserName) || authInfo?.UserSEQ <= 0)
            {
                return false;
            }

            return actionLink.Equals(authInfo.ActionLink, StringComparison.OrdinalIgnoreCase);
        }
    }
}