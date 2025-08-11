namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    public class CurrencyDistributionCriteria : CurrencyPerformanceCriteria
    {
        public byte DrillDown { get; set; } = DrillDownConst.DRILL_DOWN;

        public override string GetSettingKey()
        {
            return "LAP_Performance_CurrencyMeasureDistributionBySite";
        }
    }
}