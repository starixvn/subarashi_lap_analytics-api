using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Core.Models.ClientParams
{
    public class ComparisonPeriodParam : IToParamValue
    {
        public bool IsPreviousPeriod { get; set; }
        public int SamePeriodOfYear { get; set; }

        public IDictionary<string, object> ToParamValue()
        {
            throw new System.NotImplementedException();
        }
    }
}