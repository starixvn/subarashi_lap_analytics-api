namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    using System;
    using System.Collections.Generic;

    public class ContextService : IContextService
    {
        public IDictionary<string, string> Errors { get; protected set; }

        public IDictionary<string, string> Info { get; protected set; }

        public void AddError(string key, string message) =>
            (Errors ?? (Errors = new Dictionary<string, string>()))
                .Add(key, message);

        public void AddError(string message)
        {
            var key = Guid.NewGuid().ToString();
            (Errors ?? (Errors = new Dictionary<string, string>()))
                .Add(key, message);
        }

        public void AddInfo(string key, string message) =>
            (Info ?? (Info = new Dictionary<string, string>()))
                .Add(key, message);

        public void AddInfo(string message)
        {
            var key = Guid.NewGuid().ToString();
            (Info ?? (Info = new Dictionary<string, string>()))
                .Add(key, message);
        }

        public string Message
        {
            get
            {
                var msg = string.Empty;

                if (Errors != null && Errors.Count > 0)
                {
                    msg = string.Join(", ", Errors.Values);
                }
                else if (Info != null && Info.Count > 0)
                {
                    msg = string.Join(", ", Info.Values);
                }

                //nothing
                return msg;
            }
        }
    }
}