using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;

namespace Fanex.Athena.AnalyticsAPI.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class OneAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var roles = Roles?.Split(',') ?? Enumerable.Empty<string>();

            var isAuthorized = user.IsInRole(PermissionConst.SuperAdminRole) || roles.Any(_ => user.IsInRole(_));

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