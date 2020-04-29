using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.Models;
using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using LAP.ONEBook.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.ONEBookAuth
{
    public class ONEBookAuthenticationHandler : AuthenticationHandler<ONEBookAuthenticationOptions>
    {
        public ONEBookAuthenticationHandler(IOptionsMonitor<ONEBookAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Options.AllowBypass)
            {
                var authInfo = Options.LazyFakeData.Value as DefaultAuthInfo<Permission, CurrencyAgentInfo>;
                var ticket = CreateTicket(authInfo);
                return AuthenticateResult.Success(ticket);
            }

            var token = Options.TokenRetrieval?.Invoke(Request);
            if (!string.IsNullOrEmpty(token))
            {
                var authInfo = await Options.AuthClient.Value.AuthenticateAsync(token).ConfigureAwait(false);
                if (ValidateToken(authInfo, Request.Path))
                {
                    var ticket = CreateTicket(authInfo);
                    return AuthenticateResult.Success(ticket);
                }
            }
            return AuthenticateResult.NoResult();
        }

        private AuthenticationTicket CreateTicket(DefaultAuthInfo<Permission, CurrencyAgentInfo> authInfo)
        {
            var roles = authInfo.IsAdminstrator ? new[] { PermissionConst.SUPER_ADMIN } :
                        authInfo.Permissions.Select(_ => _.FunctionKey).ToArray();

            var identityClaim = new Claim(ClaimTypes.Name, authInfo.SiteName);
            var userDataClaim = new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(authInfo));
            var claims = new ClaimsIdentity(new[] { identityClaim, userDataClaim }, Scheme.Name);
            var principal = new GenericPrincipal(claims, roles);

            return new AuthenticationTicket(principal, Scheme.Name);
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