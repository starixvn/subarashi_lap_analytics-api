namespace Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels
{
    public interface IDateRangeMetaModel
    {
        string FromDate { get; set; }

        string ToDate { get; set; }
    }

    public interface IComparisonPeriodMetaModel
    {
        ComparisonPeriodMetaModel ComparePeriod { get; set; }
    }

    public interface IDateRangeCompare : IDateRangeMetaModel, IComparisonPeriodMetaModel
    {
        void UpdateDateRangeByComparePeriod();
    }

    public class ComparisonPeriodMetaModel
    {
        public bool? IsPreviousPeriod { get; set; }
        public int? SamePeriodOfYear { get; set; }
    }
}