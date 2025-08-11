namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.MetaModels;
    using Models.ViewModels;

    public interface IProductPerformanceService : IPerformanceService, IPerformanceDistributionService
    {
        Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(ProductPerformanceFilterMetaModel filter);

        Task<IEnumerable<MeasurementPeriodComparisonGrowthVM>> GetPeriodGrowthWithComparison(ProductPerformanceFilterMetaModel filter);

        Task<IDictionary<string, object>> GetAllDetailMonthGrowthWithComparison(ProductPerformanceFilterMetaModel filter);

        Task<IEnumerable<OverallMeasurementVM>> GetOverallMeasurement(PerformanceFilterMetaModel filter);

        Task<IEnumerable<MeasurementComparisonVM>> GetMeasurementDifferenceWithComparison(ProductPerformanceFilterMetaModel filter);
    }
}