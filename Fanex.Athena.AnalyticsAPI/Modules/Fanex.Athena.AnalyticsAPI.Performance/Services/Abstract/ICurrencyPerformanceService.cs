using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public interface ICurrencyPerformanceService : IPerformanceService
    {
        Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(CurrenyPerformanceFilterMetaModel filter);

        Task<IEnumerable<MeasurementComparisonVM>> GetMeasurementDifferenceWithComparison(CurrenyPerformanceFilterMetaModel filter);

        Task<IEnumerable<MeasurementPeriodComparisonGrowthVM>> GetPeriodGrowthWithComparison(CurrenyPerformanceFilterMetaModel filter);

        Task<IEnumerable<MeasurementGrowthVM>> GetDetailMonthGrowthWithComparison(CurrenyPerformanceFilterMetaModel filter);

        Task<IEnumerable<OverallMeasurementVM>> GetOverallMeasurement(PerformanceFilterMetaModel filter);

        Task<IDictionary<string, object>> GetAllDetailMonthGrowthWithComparison(CurrenyPerformanceFilterMetaModel filter);
    }
}