using Fanex.Athena.AnalyticsAPI.Common.Models.Criteria;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    public abstract class CurrencyPerformanceCriteria : AnalyticCriteria
    {
        public string ListCurrency { get; set; }
    }
}