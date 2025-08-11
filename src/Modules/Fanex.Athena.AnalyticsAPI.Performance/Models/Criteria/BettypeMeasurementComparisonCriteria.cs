namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    public class BettypeMeasurementComparisonCriteria : BettypePerformanceCriteria
    {
        public byte DrillDown { get; set; } = DrillDownConst.DRILL_DOWN;

        public override string GetSettingKey()
        {
            return "LAP_Performance_BettypeComparionPeriodBySite";
        }
    }
}