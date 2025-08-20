namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    using Common.Models.Criteria;

    public class OverviewMeasurementGrowthCriteria : AnalyticCriteria, IMeasurementCriteria
    {
        public int MeasurementId { get; set; }

        public byte GroupID { get; set; }

        public override string GetSettingKey()
        {
            return "LAP_Performance_OverviewMeasurementGrowthBySite";
        }
    }
}