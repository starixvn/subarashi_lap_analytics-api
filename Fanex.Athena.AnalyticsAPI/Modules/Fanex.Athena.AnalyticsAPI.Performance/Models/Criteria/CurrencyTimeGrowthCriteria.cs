namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    public class CurrencyTimeGrowthCriteria : CurrencyPerformanceCriteria, IMeasurementCriteria
    {
        public int MeasurementId { get; set; }

        public byte GroupID { get; set; } = GroupDataConst.MONTH_GROUP;

        public override string GetSettingKey()
        {
            return "LAP_Performance_CurrencyComparionTimeBySite";
        }
    }
}