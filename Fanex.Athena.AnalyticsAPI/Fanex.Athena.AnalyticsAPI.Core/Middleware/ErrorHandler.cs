using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Middleware
{
    public class ErrorHandler
    {
        private const int ServerErrorCode = 500;
        private readonly RequestDelegate _next;

        public ErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

            if (error != null && error.Error != null)
            {
                await HandleError(context, error.Error);
            }

            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                await HandleError(context, exception);
            }
        }

        private async Task HandleError(HttpContext context, Exception error)
        {
            var body = GetBodyContent(context);
            context.Response.StatusCode = ServerErrorCode;
            context.Response.ContentType = "application/json";
            var errorMsg = $"Msg={error.Message}\n Stacktrace={error.StackTrace}" +
                $"\n Path={context.Request.Path}\n Query={context.Request.QueryString}\n Body={body}";

            Logging.Logger.Log.Error(errorMsg);

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                state = "Internal Server Error",
                msg = "Sorry, an error occurred while processing your request.",
                isSuccess = false
            }));
        }

        private static string GetBodyContent(HttpContext context)
        {
            var body = string.Empty;
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(context.Request.Body))
            {
                body = reader.ReadToEnd();
            }
            return body;
        }
    }
}