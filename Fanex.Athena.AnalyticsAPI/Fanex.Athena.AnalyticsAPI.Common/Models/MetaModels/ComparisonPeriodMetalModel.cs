using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels
{
    public class ComparisonPeriodMetalModel
    {
        public bool IsPreviousPeriod { get; set; }
        public int SamePeriodOfYear { get; set; }

        public IDictionary<string, object> ToParamValue()
        {
            return null;
        }
    }
}