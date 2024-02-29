using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ONEbook.UM.Services;

namespace Fanex.Athena.AnalyticsAPI.Core.Middleware
{
    public class UnderMaintenanceHandler
    {
        private const string MsgFormat = "System is currently unavailable due to scheduled maintenance. This downtime will last from {0} to {1} (HK Time GMT +8)";

        private readonly RequestDelegate _next;
        private readonly IMaintenanceService _maintenanceService;

        public UnderMaintenanceHandler(RequestDelegate next, IMaintenanceService maintenanceService)
        {
            _next = next;
            _maintenanceService = maintenanceService;
        }

        public async Task Invoke(HttpContext context)
        {
            var isUM = await _maintenanceService.IsUnderMaintenanceAsync();
            if (isUM)
            {
                var umTime = await _maintenanceService.GetUnderMaintenanceTimeAsync();
                string msg;
                string contentType;
                contentType = "application/json";
                msg = JsonConvert.SerializeObject(new
                {
                    msg = string.Format(MsgFormat, umTime.From, umTime.To),
                    isSuccess = false
                });

                context.Response.ContentType = contentType;
                await context.Response.WriteAsync(msg);
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}