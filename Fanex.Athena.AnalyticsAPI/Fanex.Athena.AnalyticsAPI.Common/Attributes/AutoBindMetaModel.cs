using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fanex.Athena.AnalyticsAPI.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AutoBindMetaModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var metaModel = context.ActionArguments.FirstOrDefault(x => x.Value is IBaseMetaModel)
                .Value as IBaseMetaModel;
            if (metaModel != null)
            {
                metaModel.SiteName = context.HttpContext.User.Identity.Name;
            }
        }
    }
}
