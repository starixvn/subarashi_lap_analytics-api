using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.ONEBookAuth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ONEBookAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public string Permissions { get; set; }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var isAuthorized = context.HttpContext.User.Identity.IsAuthenticated && IsAuthorized(context.HttpContext);
            if (!isAuthorized)
            {
                context.Result = RequestResultConst.ForbiddenResult;
            }

            return Task.CompletedTask;
        }

        protected virtual bool IsAuthorized(HttpContext context)
        {
            return IsInPermissions(context, Permissions);
        }

        private static bool IsInPermissions(HttpContext context, string permissions)
        {
            var user = context.User;
            if (user.IsInRole(PermissionConst.SUPER_ADMIN))
            {
                return true;
            }

            var listPermissions = permissions?.Split(',') ?? Enumerable.Empty<string>();
            return listPermissions.Any(_ => user.IsInRole(_));
        }
    }
}