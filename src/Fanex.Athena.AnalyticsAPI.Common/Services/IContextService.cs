namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    using System.Collections.Generic;

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