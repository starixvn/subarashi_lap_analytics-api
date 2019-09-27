using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonHelper = Fanex.Athena.AnalyticsAPI.Common.Helpers;
using PerformanceHelper = Fanex.Athena.AnalyticsAPI.Performance.Helpers;

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

        public async Task<IEnumerable<MeasurementGrowthVM>> GetPeriodGrowthWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevOfPrevPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewMeasurementGrowthCriteria, PerformanceFilterMetaModel>(filter);
            criteriaPrevOfPrevPeriod.GroupID = GroupData.NONE_GROUP;
            var taskPrevOfPrevPeriod = _performanceRepository.QueryAsync<TotalDTO>(criteriaPrevOfPrevPeriod);

            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewMeasurementGrowthCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.GroupID = GroupData.NONE_GROUP;
            var taskPrevPeriod = _performanceRepository.QueryAsync<TotalDTO>(criteriaPrevPeriod);

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewMeasurementGrowthCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.GroupID = GroupData.NONE_GROUP;
            var taskCurrentPeriod = _performanceRepository.QueryAsync<TotalDTO>(criteriaCurrentPeriod);

            var taskResults = await Task.WhenAll(taskPrevOfPrevPeriod, taskPrevPeriod, taskCurrentPeriod);

            return null;
        }

        public async Task<IEnumerable<MeasurementGrowthVM>> GetMonthGrowthWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaSelectedPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewMeasurementGrowthCriteria, PerformanceFilterMetaModel>(filter);
            criteriaSelectedPeriod.GroupID = GroupData.MONTH_GROUP;
            var taskSelectedPeriod = _performanceRepository.QueryAsync<MeasurementGrowthDTO>(criteriaSelectedPeriod);

            var criteriaComparePeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewMeasurementGrowthCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaSelectedPeriod.GroupID = GroupData.MONTH_GROUP;

            var taskComparePeriod = _performanceRepository.QueryAsync<MeasurementGrowthDTO>(criteriaComparePeriod);

            var taskResults = await Task.WhenAll(taskSelectedPeriod, taskComparePeriod);

            var result = taskResults[0].Select(_ => new MeasurementGrowthVM
            {
                Time = _.Time.ToLocalTime().ToString(CommonHelper.DateTimeHelper.MONTH_FORMAT),
                Total = _.Total
            });

            return result;
        }

        public async Task<IEnumerable<SummaryVM>> GetSummaryWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaSelectedPeriod = CommonHelper.CriteriaHelper.ToCriteria<OverviewSummaryCriteria, PerformanceFilterMetaModel>(filter);
            var taskSelectedPeriod = _performanceRepository.QueryAsync<SummaryDTO>(criteriaSelectedPeriod);

            var criteriaComparePeriod = CommonHelper.CriteriaHelper.ToCriteria<OverviewSummaryCriteria, PerformanceFilterMetaModel>(filter, false);
            var taskComparePeriod = _performanceRepository.QueryAsync<SummaryDTO>(criteriaComparePeriod);

            var taskResults = await Task.WhenAll(taskSelectedPeriod, taskComparePeriod);

            var selectedPeriodData = taskResults[0];
            var comparePeriodData = taskResults[1];

            return null;
        }

        public async Task<IEnumerable<TopChangeVM>> GetCurrencyTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaSelectedPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaSelectedPeriod.Dimension = Dimension.CURRENCY;
            var taskSelectedPeriod = _performanceRepository.QueryAsync<CurrencyTopChangeDTO>(criteriaSelectedPeriod);

            var criteriaComparePeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaComparePeriod.Dimension = Dimension.CURRENCY;
            var taskComparePeriod = _performanceRepository.QueryAsync<CurrencyTopChangeDTO>(criteriaComparePeriod);

            var taskResults = await Task.WhenAll(taskSelectedPeriod, taskComparePeriod);

            var result = new List<TopChangeVM>
            {
            };

            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetBettypeTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaSelectedPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaSelectedPeriod.Dimension = Dimension.BETTYPE;
            var taskSelectedPeriod = _performanceRepository.QueryAsync<BettypeTopChangeDTO>(criteriaSelectedPeriod);

            var criteriaComparePeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaComparePeriod.Dimension = Dimension.BETTYPE;
            var taskComparePeriod = _performanceRepository.QueryAsync<BettypeTopChangeDTO>(criteriaComparePeriod);

            var taskResults = await Task.WhenAll(taskSelectedPeriod, taskComparePeriod);

            var result = new List<TopChangeVM>
            {
            };
            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetPlatformTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaSelectedPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaSelectedPeriod.Dimension = Dimension.PLATFORM;
            var taskSelectedPeriod = _performanceRepository.QueryAsync<PlatformTopChangeDTO>(criteriaSelectedPeriod);

            var criteriaComparePeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaComparePeriod.Dimension = Dimension.PLATFORM;
            var taskComparePeriod = _performanceRepository.QueryAsync<PlatformTopChangeDTO>(criteriaComparePeriod);

            var taskResults = await Task.WhenAll(taskSelectedPeriod, taskComparePeriod);
            
            var result = new List<TopChangeVM>
            {
            };
            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetProductTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaSelectedPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaSelectedPeriod.Dimension = Dimension.PRODUCT;
            var taskSelectedPeriod = _performanceRepository.QueryAsync<ProductTopChangeDTO>(criteriaSelectedPeriod);

            var criteriaComparePeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaComparePeriod.Dimension = Dimension.PRODUCT;
            var taskComparePeriod = _performanceRepository.QueryAsync<ProductTopChangeDTO>(criteriaComparePeriod);

            var taskResults = await Task.WhenAll(taskSelectedPeriod, taskComparePeriod);

            var result = new List<TopChangeVM>
            {
            };
            return result;
        }
    }
}