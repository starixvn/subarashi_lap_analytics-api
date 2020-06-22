using Microsoft.AspNetCore.Http;
using System.Net;

namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    public interface IClientService
    {
        IPAddress RealClientIP { get; }

        HttpContext Context { get; }
    }
}