namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    public class BettypeGrowthCriteria : BettypePerformanceCriteria
    {
        public byte GroupBy { get; set; } = GroupByConst.DIMENSION_GROUP;

        public override string GetSettingKey()
        {
            return "LAP_Performance_BettypeComparionBySite";
        }
    }
}