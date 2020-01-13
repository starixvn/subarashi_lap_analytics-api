using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.Models;
using LAP.ONEBook.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
                        new[] { PermissionConst.SUPER_ADMIN_ROLE
                        });

        private readonly RequestDelegate _next;
        private readonly IAuth<DefaultAuthInfo<Permission, dynamic>> _authService;
        private readonly IHostingEnvironment _env;

        public OneAuthenticationHandler(RequestDelegate next,
            IAuth<DefaultAuthInfo<Permission, dynamic>> authService,
            IHostingEnvironment env)
        {
            _next = next;
            _authService = authService;
            _env = env;
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
            else
            {
                var existAuthToken = context.Request.Headers.TryGetValue(AuthConst.AUTH_KEY, out StringValues token);
                if (existAuthToken)
                {
                    var authInfo = await _authService.AuthenticateAsync(token[0]).ConfigureAwait(false);
                    if (ValidateToken(authInfo, context.Request.Path))
                    {
                        var roles = authInfo.IsAdminstrator ? new[] { PermissionConst.SUPER_ADMIN_ROLE } :
                            authInfo.Permissions.Select(_ => _.FunctionKey).ToArray();

                        var identityClaim = new Claim(ClaimTypes.Name, authInfo.SiteName);

                        context.User = new GenericPrincipal(new ClaimsIdentity(new[] { identityClaim }), roles);

                        isAuthorized = true;
                    }
                }
            }

            return isAuthorized;
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