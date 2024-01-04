using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Core.Constants
{
    public static class RequestResultConst
    {
        public static readonly IActionResult BadResult = new JsonResult(new
        {
            msg = "400 Bad Request",
            isSuccess = false
        })
        { StatusCode = (int)HttpStatusCode.BadRequest };

        public static readonly IActionResult ForbiddenResult = new JsonResult(new
        {
            msg = "403 Forbidden",
            isSuccess = false
        })
        { StatusCode = (int)HttpStatusCode.Forbidden };
    }
}