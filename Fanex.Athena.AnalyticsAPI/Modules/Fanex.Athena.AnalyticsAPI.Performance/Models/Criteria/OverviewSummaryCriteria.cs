using Fanex.Athena.AnalyticsAPI.Common.Models.Criteria;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    public class OverviewSummaryCriteria : AnalyticCriteria
    {
        public override string GetSettingKey()
        {
            return "LAP_Performance_OverviewSummaryBySite";
        }
    }
}