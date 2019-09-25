using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class OverviewPerformanceService : PerformanceService, IOverviewPerformanceService
    {
        protected readonly ICalculationService _calculationService;

        public OverviewPerformanceService(
            IContextService context,
            IPerformanceRepository performanceRepository,
            ICalculationService calculationService
            ) : base(context, performanceRepository)
        {
            _calculationService = calculationService;
        }

        public async Task<IEnumerable<MeasurementGrowthVM>> GetGrowthWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaSelectedPeriod = filter.ToCriteria<OverviewMeasurementGrowthCriteria>();
            var taskSelectedPeriod = _performanceRepository.QueryAsync<MeasurementGrowthDTO>(criteriaSelectedPeriod);

            filter.UpdateDateRangeByComparePeriod();
            var criteriaComparePeriod = filter.ToCriteria<OverviewMeasurementGrowthCriteria>();
            var taskComparePeriod = _performanceRepository.QueryAsync<MeasurementGrowthDTO>(criteriaComparePeriod);

            var taskResults = await Task.WhenAll(taskSelectedPeriod, taskComparePeriod);

            var result = taskResults[0].Select(_ => new MeasurementGrowthVM
            {
                Time = _.Time.ToString(),
                Total = _.Total,
            });

            return result;
        }

        public async Task<IEnumerable<SummaryVM>> GetSummaryWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaSelectedPeriod = filter.ToCriteria<OverviewSummaryCriteria>();
            var taskSelectedPeriod = _performanceRepository.QueryAsync<SummaryDTO>(criteriaSelectedPeriod);

            filter.UpdateDateRangeByComparePeriod();
            var criteriaComparePeriod = filter.ToCriteria<OverviewSummaryCriteria>();
            var taskComparePeriod = _performanceRepository.QueryAsync<SummaryDTO>(criteriaComparePeriod);

            var taskResults = await Task.WhenAll(taskSelectedPeriod, taskComparePeriod);

            var selectedPeriodData = taskResults[0]?.FirstOrDefault();
            var comparePeriodData = taskResults[1]?.FirstOrDefault();

            var result = new List<SummaryVM>
            {
                new SummaryVM
                {
                    MeasurementId = 0,
                    Value = selectedPeriodData?.Turnover,
                    ChangePercent = _calculationService.CalculateGrowth(selectedPeriodData?.Turnover, comparePeriodData?.Turnover)
                },
                new SummaryVM
                {
                    MeasurementId = 1,
                    Value = selectedPeriodData?.GrossProfit,
                    ChangePercent = _calculationService.CalculateGrowth(selectedPeriodData?.GrossProfit, comparePeriodData?.GrossProfit)
                },
                new SummaryVM
                {
                    MeasurementId = 2,
                    Value = selectedPeriodData?.Margin,
                    ChangePercent = _calculationService.CalculateGrowth(selectedPeriodData?.Margin, comparePeriodData?.Margin)
                }
            };

            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetCurrencyTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaSelectedPeriod = filter.ToCriteria<OverviewTopChangeCriteria>();
            criteriaSelectedPeriod.Dimension = 0;
            var taskSelectedPeriod = _performanceRepository.QueryAsync<CurrencyTopChangeDTO>(criteriaSelectedPeriod);

            filter.UpdateDateRangeByComparePeriod();
            var criteriaComparePeriod = filter.ToCriteria<OverviewTopChangeCriteria>();
            criteriaComparePeriod.Dimension = 0;
            var taskComparePeriod = _performanceRepository.QueryAsync<CurrencyTopChangeDTO>(criteriaComparePeriod);

            var taskResults = await Task.WhenAll(taskSelectedPeriod, taskComparePeriod);

            var result = new List<TopChangeVM>
            {
            };
            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetBettypeTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaSelectedPeriod = filter.ToCriteria<OverviewTopChangeCriteria>();
            criteriaSelectedPeriod.Dimension = 1;
            var taskSelectedPeriod = _performanceRepository.QueryAsync<BettypeTopChangeDTO>(criteriaSelectedPeriod);

            filter.UpdateDateRangeByComparePeriod();
            var criteriaComparePeriod = filter.ToCriteria<OverviewTopChangeCriteria>();
            criteriaComparePeriod.Dimension = 1;
            var taskComparePeriod = _performanceRepository.QueryAsync<BettypeTopChangeDTO>(criteriaComparePeriod);

            var taskResults = await Task.WhenAll(taskSelectedPeriod, taskComparePeriod);

            var result = new List<TopChangeVM>
            {
            };
            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetPlatformTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaSelectedPeriod = filter.ToCriteria<OverviewTopChangeCriteria>();
            criteriaSelectedPeriod.Dimension = 2;
            var taskSelectedPeriod = _performanceRepository.QueryAsync<PlatformTopChangeDTO>(criteriaSelectedPeriod);

            filter.UpdateDateRangeByComparePeriod();
            var criteriaComparePeriod = filter.ToCriteria<OverviewTopChangeCriteria>();
            criteriaComparePeriod.Dimension = 2;
            var taskComparePeriod = _performanceRepository.QueryAsync<PlatformTopChangeDTO>(criteriaComparePeriod);

            var taskResults = await Task.WhenAll(taskSelectedPeriod, taskComparePeriod);

            var result = new List<TopChangeVM>
            {
            };
            return result;
        }
    }
}