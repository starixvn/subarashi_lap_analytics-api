using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using System;
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

        public async Task<IEnumerable<MeasurementGrowthVM>> GetGrowthResultWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteria = filter.ToCriteria<OverviewMeasurementGrowthCriteria>();
            var dto = await _performanceRepository.QueryAsync<MeasurementGrowthDTO>(criteria);
            var result = dto.Select(_ => new MeasurementGrowthVM
            {
            });

            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetTopChangeResultWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteria = filter.ToCriteria<OverviewTopChangeCriteria>();

            criteria.Dimension = 0;
            var currencyDim = await _performanceRepository.QueryAsync<CurrencyTopChangeDTO>(criteria);

            criteria.Dimension = 1;
            var bettypeDim = await _performanceRepository.QueryAsync<BettypeTopChangeDTO>(criteria);

            criteria.Dimension = 2;
            var platformDim = await _performanceRepository.QueryAsync<PlatformTopChangeDTO>(criteria);

            return null;
        }

        public async Task<IEnumerable<SummaryVM>> GetSummaryResultWithComparison(PerformanceFilterMetaModel filter)
        {
            //
            var criteria = filter.ToCriteria<OverviewSummaryCriteria>();
            var summary = await _performanceRepository.QueryAsync<SummaryDTO>(criteria);
            var selectedData = summary.FirstOrDefault();

            //
            filter.UpdateDateRangeByComparisonPeriod();
            criteria = filter.ToCriteria<OverviewSummaryCriteria>();
            summary = await _performanceRepository.QueryAsync<SummaryDTO>(criteria);
            var prevData = summary.FirstOrDefault();

            var result = new List<SummaryVM>
            {
                new SummaryVM
                {
                    MeasurementId = 0,
                    Value = selectedData?.Turnover,
                    ChangePercent = _calculationService.CalculateGrowth(selectedData?.Turnover, prevData?.Turnover)
                },
                new SummaryVM
                {
                    MeasurementId = 1,
                    Value = selectedData?.GrossProfit,
                    ChangePercent = _calculationService.CalculateGrowth(selectedData?.GrossProfit, prevData?.GrossProfit)
                },
                new SummaryVM
                {
                    MeasurementId = 2,
                    Value = selectedData?.Margin,
                    ChangePercent = _calculationService.CalculateGrowth(selectedData?.Margin, prevData?.Margin)
                }
            };

            return result;
        }
    }
}