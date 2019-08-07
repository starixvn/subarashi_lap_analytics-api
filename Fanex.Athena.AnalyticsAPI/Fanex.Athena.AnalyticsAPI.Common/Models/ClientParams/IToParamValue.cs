using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.ClientParams
{
    public interface IToParamValue
    {
        IDictionary<string, object> ToParamValue();
    }
}