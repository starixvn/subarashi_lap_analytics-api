namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    using Common.Models.Criteria;

    public abstract class ProductPerformanceCriteria : AnalyticCriteria, IMeasurementCriteria
    {
        public string ListProduct { get; set; }

        public byte SortByMeasurement { get; set; } = SortByMeasurementConst.NONE_SORT;

        public int MeasurementId { get; set; }
    }
}