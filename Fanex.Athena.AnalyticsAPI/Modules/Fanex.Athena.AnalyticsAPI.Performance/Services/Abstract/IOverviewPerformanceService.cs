namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.MetaModels;
    using Models.ViewModels;

    public interface IOverviewPerformanceService
    {
        Task<IEnumerable<SummaryVM>> GetSummaryWithComparison(PerformanceFilterMetaModel filter);

        Task<IEnumerable<TopChangeVM>> GetCurrencyTopChangeWithComparison(PerformanceFilterMetaModel filter);

        Task<IEnumerable<TopChangeVM>> GetBettypeTopChangeWithComparison(PerformanceFilterMetaModel filter);

        Task<IEnumerable<TopChangeVM>> GetPlatformTopChangeWithComparison(PerformanceFilterMetaModel filter);

        Task<IEnumerable<TopChangeVM>> GetProductTopChangeWithComparison(PerformanceFilterMetaModel filter);

        Task<IEnumerable<MeasurementGrowthVM>> GetPeriodGrowthWithComparison(PerformanceFilterMetaModel filter);

        Task<IEnumerable<MeasurementGrowthVM>> GetDetailMonthGrowthWithComparison(PerformanceFilterMetaModel filter);
    }
}