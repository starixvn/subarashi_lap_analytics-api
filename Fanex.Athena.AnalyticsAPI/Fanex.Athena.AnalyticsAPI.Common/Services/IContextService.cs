using Microsoft.AspNetCore.Http;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface IContextService
    {
        void AddError(string key, string message);

        void AddInfo(string key, string message);

        string GetError();

        string GetInfo();

        HttpContext Context { get; set; }
    }
}