using Fanex.Athena.AnalyticsAPI.Common.Models.Criteria;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    public class OverviewMeasurementGrowthCriteria : AnalyticCriteria
    {
        public int MeasurementId { get; set; }

        public override string GetSettingKey()
        {
            return "LAP_Performance_OverviewMeasurementGrowthBySite";
        }
    }
}