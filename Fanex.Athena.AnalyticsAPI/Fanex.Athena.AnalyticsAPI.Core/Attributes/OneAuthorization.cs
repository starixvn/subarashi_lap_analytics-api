using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;

namespace Fanex.Athena.AnalyticsAPI.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class OneAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var roles = Roles?.Split(',') ?? Enumerable.Empty<string>();

            var isAuthorized = roles.Contains("PermissionConst.SupperAdminRole") || roles.Any(_ => user.IsInRole(_));

            if (!isAuthorized)
            {
                var content = new
                {
                    msg = "403 Forbidden",
                    isSuccess = false
                };

                context.Result = new JsonResult(content) { StatusCode = (int)HttpStatusCode.Forbidden };
            }
        }
    }
}