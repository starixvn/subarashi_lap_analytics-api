namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    using Common.Models.Criteria;

    public class OverviewSummaryCriteria : AnalyticCriteria
    {
        public override string GetSettingKey()
        {
            return "LAP_Performance_OverviewSummaryBySite";
        }
    }
}