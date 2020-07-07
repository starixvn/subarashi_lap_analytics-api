using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Net;

namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    public class ClientService : IClientService
    {
        private const string ForwardedForHeader = "X-Forwarded-For";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IPAddress RealClientIP
        {
            get
            {
                if (Context.Request.Headers.TryGetValue(ForwardedForHeader, out StringValues values)
                    && IPAddress.TryParse(GetFirstIP(values), out IPAddress address))
                {
                    return address;
                }
                return Context.Connection.RemoteIpAddress;
            }
        }

        public HttpContext Context => _httpContextAccessor.HttpContext;

        private static string GetFirstIP(StringValues values)
        {
            var firstIP = values.FirstOrDefault()?
                .Split(',')
                .LastOrDefault()?
                .Trim();

            return firstIP;
        }
    }
}