using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Core.Models.ClientParams
{
    public interface IToParamValue
    {
        IDictionary<string, object> ToParamValue();
    }
}