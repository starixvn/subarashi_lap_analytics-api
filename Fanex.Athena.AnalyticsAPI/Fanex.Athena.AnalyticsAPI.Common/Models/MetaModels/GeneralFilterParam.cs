namespace Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels
{
    public class GeneralFilterParam : BaseParam
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public SelectParam BaseCurrency { get; set; }
        public ComparisonPeriodParam ComparisonPeriod { get; set; }
    }
}