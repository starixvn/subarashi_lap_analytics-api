namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    public class CurrencyGrowthCriteria : CurrencyPerformanceCriteria
    {
        public byte GroupBy { get; set; } = GroupByConst.DIMENSION_GROUP;

        public override string GetSettingKey()
        {
            return "LAP_Performance_CurrencyComparionBySite";
        }
    }
}