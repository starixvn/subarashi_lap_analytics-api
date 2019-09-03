using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Core.Models.ClientParams
{
    public class SelectParam : IToParamValue
    { 
        public int Id { get; set; }
        public string Name { get; set; }

        public IDictionary<string, object> ToParamValue()
        {
            throw new System.NotImplementedException();
        }
    }
}