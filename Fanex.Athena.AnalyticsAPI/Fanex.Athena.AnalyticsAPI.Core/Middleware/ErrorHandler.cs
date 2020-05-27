using Fanex.Athena.AnalyticsAPI.Core.Helpers;
using Fanex.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Middleware
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandler(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
            if (error != null && error.Error != null)
            {
                await HandleError(context, error.Error);
            }
            else
            {
                try
                {
                    await _next.Invoke(context);
                }
                catch (Exception exception)
                {
                    await HandleError(context, exception);
                }
            }
        }

        private async Task HandleError(HttpContext context, Exception error)
        {
            var errorMsg = ErrorMessageHelper.GenerateMessage(context, error);
            await _logger.ErrorAsync(errorMsg);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                state = "Internal Server Error",
                msg = "Sorry, an error occurred while processing your request.",
                isSuccess = false
            }));
        }
    }
}