using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface IContextService
    {
        IDictionary<string, string> Errors { get; }

        IDictionary<string, string> Info { get; }

        void AddError(string key, string message);

        void AddError(string message);

        void AddInfo(string key, string message);

        void AddInfo(string message);

        string Message { get; }
    }
}