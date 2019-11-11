namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    public class CurrencyPeriodGrowthCriteria : CurrencyPerformanceCriteria, IMeasurementCriteria
    {
        public int MeasurementId { get; set; }

        public byte DrillDown { get; set; } = DrillDownConst.DRILL_DOWN;

        public override string GetSettingKey()
        {
            return "LAP_Performance_CurrencyComparionCurrencyBySite";
        }
    }
}