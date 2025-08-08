namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.MetaModels;
    using Models.ViewModels;

    public interface IBettypePerformanceService : IPerformanceService, IPerformanceDistributionService
    {
        Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(BettypePerformanceFilterMetaModel filter);

        Task<IEnumerable<MeasurementPeriodComparisonGrowthVM>> GetPeriodGrowthWithComparison(BettypePerformanceFilterMetaModel filter);

        Task<IDictionary<string, object>> GetAllDetailMonthGrowthWithComparison(BettypePerformanceFilterMetaModel filter);

        Task<IEnumerable<OverallMeasurementVM>> GetOverallMeasurement(PerformanceFilterMetaModel filter);

        Task<IEnumerable<MeasurementComparisonVM>> GetMeasurementDifferenceWithComparison(BettypePerformanceFilterMetaModel filter);
    }
}