using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Core.Models.DTO;
using LAP.ONEBook.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Fanex.Athena.AnalyticsAPI.Core.Middleware
{
    public class OneAuthHandler
    {
        const string AUTHKEY = "X-Auth-Token";
        private readonly RequestDelegate _next;
        private readonly IStorageProvider _storageProvider;

        public OneAuthHandler(RequestDelegate next, IStorageProvider storageProvider)
        {
            _next = next;
            _storageProvider = storageProvider;
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
            var isAuthorized = context.Request.Headers.TryGetValue(AUTHKEY, out StringValues token);

            if (isAuthorized)
            {
                var auth = new Auth<AuthInfo>(_storageProvider);
                var authInfo = await auth.AuthenticateAsync(token[0]).ConfigureAwait(false);
                var path = context.Request.PathBase + context.Request.Path;  

                if (ValidateToken(authInfo, path))
                {
                    context.User = new GenericPrincipal(new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, authInfo.SiteName) }),
                        new[] { "" });
                }
                else
                {
                    isAuthorized = false;
                }

            }

            return isAuthorized;
        }

        private static bool ValidateToken(AuthInfo authInfo, string actionUrl)
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
                State = "401 Unauthorized Error",
                Msg = "401 Unauthorized",
                IsSuccess = false
            }));
        }
    }
}
