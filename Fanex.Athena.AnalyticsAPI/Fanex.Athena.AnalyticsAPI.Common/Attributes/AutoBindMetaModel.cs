using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;

namespace Fanex.Athena.AnalyticsAPI.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AutoBindMetaModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var metaModel = context.ActionArguments.FirstOrDefault(x => x.Value is IBaseMetaModel)
                .Value as IBaseMetaModel;
            var contextUser = context.HttpContext.User;
            if (metaModel != null && contextUser != null)
            {
                BindSiteName(metaModel, contextUser);
                BindDefaultLanguage(metaModel, contextUser);
            }
        }

        private static void BindSiteName(IBaseMetaModel metaModel, ClaimsPrincipal user)
        {
            metaModel.SiteName = user.Identity.Name;
        }

        private static void BindDefaultLanguage(IBaseMetaModel metaModel, ClaimsPrincipal user)
        {
            var languageClaim = user.Claims.FirstOrDefault(_ => _.Type.Equals("language"));
            metaModel.LangCode = LangCodeConst.Mapper.ContainsKey(languageClaim?.Value) ?
                LangCodeConst.Mapper[languageClaim?.Value]
                : LangCodeConst.EnUS;
        }
    }
}