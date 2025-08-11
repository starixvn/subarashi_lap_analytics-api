namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.MetaModels;
    using Models.ViewModels;

    public interface IPlatformPerformanceService : IPerformanceService, IPerformanceDistributionService
    {
        Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(PlatformPerformanceFilterMetaModel filter);

        Task<IEnumerable<MeasurementPeriodComparisonGrowthVM>> GetPeriodGrowthWithComparison(PlatformPerformanceFilterMetaModel filter);

        Task<IDictionary<string, object>> GetAllDetailMonthGrowthWithComparison(PlatformPerformanceFilterMetaModel filter);

        Task<IEnumerable<OverallMeasurementVM>> GetOverallMeasurement(PerformanceFilterMetaModel filter);

        Task<IEnumerable<MeasurementComparisonVM>> GetMeasurementDifferenceWithComparison(PlatformPerformanceFilterMetaModel filter);
    }
}