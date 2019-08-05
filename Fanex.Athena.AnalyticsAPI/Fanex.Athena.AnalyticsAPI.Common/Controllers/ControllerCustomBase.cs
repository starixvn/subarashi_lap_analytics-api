using Fanex.Athena.AnalyticsAPI.Common.Models.DataDomain;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Common.Controllers
{
    public class ControllerCustomBase : ControllerBase
    {
        protected JsonResult ToResult<T>(T data)
        {
            return new JsonResult(new ResponseDataModel<T>(data));
        }

        protected JsonResult ToResult()
        {
            return new JsonResult(new ResponseDataModel());
        }
    }
}