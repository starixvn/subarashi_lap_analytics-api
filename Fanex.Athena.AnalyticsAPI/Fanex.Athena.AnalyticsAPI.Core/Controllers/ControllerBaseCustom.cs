using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using Fanex.Athena.AnalyticsAPI.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.AnalyticsAPI.Core.Controllers
{
    public class ControllerBaseCustom : ControllerBase
    {
        [NonAction]
        protected JsonResult ToResult<T>(T data)
        {
            var service = GetService<IContextService>();
            return new JsonResult(new ResponseDataModel<T>(data, service.Errors == null || service.Errors.Count == 0, service.Message));
        }

        [NonAction]
        protected JsonResult ToResult()
        {
            var service = GetService<IContextService>();
            return new JsonResult(new ResponseDataModel(service.Errors == null || service.Errors.Count == 0, service.Message));
        }

        [NonAction]
        protected T GetService<T>()
        {
            return HttpContext.RequestServices.GetRequiredService<T>();
        }
    }
}