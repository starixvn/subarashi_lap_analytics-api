using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public interface IProductPerformanceService : IPerformanceService
    {
        Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(ProductPerformanceFilterMetaModel filter);

        Task<IEnumerable<MeasurementPeriodComparisonGrowthVM>> GetPeriodGrowthWithComparison(ProductPerformanceFilterMetaModel filter);

        Task<IDictionary<string, object>> GetAllDetailMonthGrowthWithComparison(ProductPerformanceFilterMetaModel filter);

        Task<IEnumerable<OverallMeasurementVM>> GetOverallMeasurement(PerformanceFilterMetaModel filter);

        Task<IEnumerable<MeasurementComparisonVM>> GetMeasurementDifferenceWithComparison(ProductPerformanceFilterMetaModel filter);

        Task<(IEnumerable<DistributionDTO> prevPeriodData, IEnumerable<DistributionDTO> currentPeriodData)> GetDistributionRawData(PerformanceFilterMetaModel filter);
    }
}