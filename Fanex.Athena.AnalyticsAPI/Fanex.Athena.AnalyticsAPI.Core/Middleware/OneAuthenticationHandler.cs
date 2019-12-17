using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.Models;
using Fanex.Athena.AnalyticsAPI.Core.Models.Configuration;
using LAP.ONEBook.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Middleware
{
    public class OneAuthenticationHandler
    {
        private static GenericPrincipal DEFAULT_PRINCIPAL = new GenericPrincipal(new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, "12bet") }),
                        new[] { PermissionConst.SuperAdminRole
                        });

        private const string AUTHKEY = "X-Auth-Token";

        private readonly RequestDelegate _next;
        private readonly IAuth<DefaultAuthInfo<Permission, dynamic>> _authService;
        private readonly IHostingEnvironment _env;
        private readonly ApplicationOptions _options;

        public OneAuthenticationHandler(RequestDelegate next,
            IAuth<DefaultAuthInfo<Permission, dynamic>> authService,
            IHostingEnvironment env,
            IOptions<ApplicationOptions> options)
        {
            _next = next;
            _authService = authService;
            _env = env;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var isAuthorized = await Authorize(context);
            if (isAuthorized)
            {
                await _next.Invoke(context);
            }
            else
            {
                await HandleUnAuthorizedRequest(context);
            }
        }

        private async Task<bool> Authorize(HttpContext context)
        {
            var isAuthorized = false;

            if (_env.IsDevelopment())
            {
                context.User = DEFAULT_PRINCIPAL;
                isAuthorized = true;
            }
            else if (_env.IsEnvironment("UAT"))
            {
                var ipAddress = context.Connection.RemoteIpAddress;
                var isInWhiteList = IsInWhiteList(ipAddress);
                if (isInWhiteList)
                {
                    context.User = DEFAULT_PRINCIPAL;
                    isAuthorized = true;
                }
            }
            else
            {
                isAuthorized = context.Request.Headers.TryGetValue(AUTHKEY, out StringValues token);
                if (isAuthorized)
                {
                    var authInfo = await _authService.AuthenticateAsync(token[0]).ConfigureAwait(false);
                    if (ValidateToken(authInfo, context.Request.Path))
                    {
                        var roles = authInfo.IsAdminstrator ? new[] { PermissionConst.SuperAdminRole } :
                            authInfo.Permissions.Select(_ => _.FunctionKey).ToArray();

                        var identityClaim = new Claim(ClaimTypes.Name, authInfo.SiteName);

                        context.User = new GenericPrincipal(new ClaimsIdentity(new[] { identityClaim }), roles);
                    }
                    else
                    {
                        isAuthorized = false;
                    }
                }
            }

            return isAuthorized;
        }

        private bool IsInWhiteList(IPAddress remoteIp)
        {
            var whiteListIPList = _options?.WhiteList ?? Enumerable.Empty<string>();
            if (remoteIp.IsIPv4MappedToIPv6)
            {
                remoteIp = remoteIp.MapToIPv4();
            }
            var isInWhiteListIPList = whiteListIPList.Any(x => IPAddress.Parse(x).Equals(remoteIp));

            return isInWhiteListIPList;
        }

        private static bool ValidateToken(DefaultAuthInfo<Permission, dynamic> authInfo, string actionUrl)
        {
            if (string.IsNullOrEmpty(authInfo?.SiteName) || string.IsNullOrEmpty(authInfo?.UserName) || authInfo?.UserSEQ <= 0)
            {
                return false;
            }

            return actionUrl.Equals(authInfo.ActionLink, StringComparison.OrdinalIgnoreCase);
        }

        private static async Task HandleUnAuthorizedRequest(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                msg = "401 Unauthorized",
                isSuccess = false
            }));
        }
    }
}