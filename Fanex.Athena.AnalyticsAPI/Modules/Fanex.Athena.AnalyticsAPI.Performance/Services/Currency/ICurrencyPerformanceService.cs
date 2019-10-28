using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public interface ICurrencyPerformanceService
    {
        Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(CurrenyPerformanceFilterMetalModel filter);

        Task<IEnumerable<MeasurementComparisonVM>> GetMeasurementDifferenceWithComparison(CurrenyPerformanceFilterMetalModel filter);

        Task<IEnumerable<MeasurementPeriodComparisonGrowthVM>> GetPeriodGrowthWithComparison(CurrenyPerformanceFilterMetalModel filter);

        Task<IEnumerable<MeasurementGrowthVM>> GetDetailMonthGrowthWithComparison(CurrenyPerformanceFilterMetalModel filter);

        Task<IEnumerable<(string topDistributionId, string topDistributionName)>> GetTopDistributions(PerformanceFilterMetaModel filter);

        Task<IEnumerable<OverallMeasurementVM>> GetOverallMeasurement(PerformanceFilterMetaModel filter);
    }
}