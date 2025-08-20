namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    using Common.Models.Criteria;

    public abstract class PlatformPerformanceCriteria : AnalyticCriteria, IMeasurementCriteria
    {
        public string ListPlatform { get; set; }

        public byte SortByMeasurement { get; set; } = SortByMeasurementConst.NONE_SORT;

        public int MeasurementId { get; set; }
    }
}