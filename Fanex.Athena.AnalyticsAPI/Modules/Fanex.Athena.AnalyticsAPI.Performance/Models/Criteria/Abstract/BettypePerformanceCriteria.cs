using Fanex.Athena.AnalyticsAPI.Common.Models.Criteria;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    public abstract class BettypePerformanceCriteria : AnalyticCriteria, IMeasurementCriteria
    {
        public string ListCombineBettype { get; set; }
        public byte SortByMeasurement { get; set; } = SortByMeasurementConst.NONE_SORT;
        public int MeasurementId { get; set; }
    }
}