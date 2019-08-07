using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class ContextService : IContextService
    {
        protected IDictionary<string, string> Errors { get; set; }

        protected IDictionary<string, string> Info { get; set; }

        public HttpContext Context { get; set; }

        public void AddError(string key, string message)
        {
            (Errors ?? (Errors = new Dictionary<string, string>()))
                .Add(key, message);
        }

        public void AddInfo(string key, string message)
        {
            (Info ?? (Info = new Dictionary<string, string>()))
                .Add(key, message);
        }

        public string GetError()
        {
            return Errors != null && Errors.Count > 0 
                ? string.Join(", ", Errors.Values) : null;
        }

        public string GetInfo()
        {
            throw new System.NotImplementedException();
        }
    }
}