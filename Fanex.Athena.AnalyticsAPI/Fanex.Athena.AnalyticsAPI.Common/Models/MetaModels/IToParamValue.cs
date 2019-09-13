using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels
{
    public interface IToParamValue
    {
        IDictionary<string, object> ToParamValue();
    }
}