using Fanex.Athena.AnalyticsAPI.Common.Extensions;
using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CommonHelper = Fanex.Athena.AnalyticsAPI.Common.Helpers;

using PerformanceHelper = Fanex.Athena.AnalyticsAPI.Performance.Helpers;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class OverviewPerformanceService : PerformanceService, IOverviewPerformanceService
    {
        private readonly IOverviewPerformanceVMMapper _viewModelMapper;

        public OverviewPerformanceService(
            IContextService context,
            IPerformanceRepository performanceRepository,
            IOverviewPerformanceVMMapper viewModelMapper
            ) : base(context, performanceRepository)
        {
            _viewModelMapper = viewModelMapper;
        }

        public async Task<IEnumerable<MeasurementGrowthVM>> GetPeriodGrowthWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevOfPrevPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewMeasurementGrowthCriteria, PerformanceFilterMetaModel>(filter);
            CriteriaHelper.UpdateCurrentPeriodToPreviousPeriod(criteriaPrevOfPrevPeriod, filter.PreviousPeriod, filter.CurrentPeriod);
            criteriaPrevOfPrevPeriod.GroupID = GroupData.NONE_GROUP;
            var taskPrevOfPrevPeriod = _performanceRepository.QueryAsync<TotalDTO>(criteriaPrevOfPrevPeriod);

            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewMeasurementGrowthCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.GroupID = GroupData.NONE_GROUP;
            var taskPrevPeriod = _performanceRepository.QueryAsync<TotalDTO>(criteriaPrevPeriod);

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewMeasurementGrowthCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.GroupID = GroupData.NONE_GROUP;
            var taskCurrentPeriod = _performanceRepository.QueryAsync<TotalDTO>(criteriaCurrentPeriod);

            var result = _viewModelMapper.ToViewModel(await taskPrevOfPrevPeriod, await taskPrevPeriod, await taskCurrentPeriod);

            return result;
        }

        public async Task<IEnumerable<MeasurementGrowthVM>> GetMonthGrowthWithComparison(PerformanceFilterMetaModel filter)
        {
            var newFilter = MetaModelHelper.BackToCurrentPeriodByMonth(filter, 1);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewMeasurementGrowthCriteria, PerformanceFilterMetaModel>(newFilter);
            criteriaCurrentPeriod.GroupID = GroupData.MONTH_GROUP;
            var taskCurrentPeriod = _performanceRepository.QueryAsync<MeasurementGrowthDTO>(criteriaCurrentPeriod);

            var result = _viewModelMapper.ToViewModel(
                await taskCurrentPeriod,
                DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.FromMonth),
                DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.ToMonth)
                );

            return result;
        }

        public async Task<IEnumerable<SummaryVM>> GetSummaryWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = CriteriaHelper.ToCriteria<OverviewSummaryCriteria, PerformanceFilterMetaModel>(filter, false);
            var taskPrevPeriod = _performanceRepository.QueryAsync<SummaryDTO>(criteriaPrevPeriod);

            var criteriaCurrentPeriod = CommonHelper.CriteriaHelper.ToCriteria<OverviewSummaryCriteria, PerformanceFilterMetaModel>(filter);
            var taskCurrentPeriod = _performanceRepository.QueryAsync<SummaryDTO>(criteriaCurrentPeriod);

            var result = _viewModelMapper.ToViewModel(await taskPrevPeriod, await taskCurrentPeriod);

            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetCurrencyTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.Dimension = Dimension.CURRENCY;
            var taskPrevPeriod = _performanceRepository.QueryAsync<CurrencyTopChangeDTO>(criteriaPrevPeriod);

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.Dimension = Dimension.CURRENCY;
            var taskCurrentPeriod = _performanceRepository.QueryAsync<CurrencyTopChangeDTO>(criteriaCurrentPeriod);

            var result = _viewModelMapper.ToViewModel(await taskPrevPeriod, await taskCurrentPeriod, _ => _.CurrencyId, _ => _.CurrencyName);

            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetBettypeTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.Dimension = Dimension.BETTYPE;
            var taskPrevPeriod = _performanceRepository.QueryAsync<BettypeTopChangeDTO>(criteriaPrevPeriod);

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.Dimension = Dimension.BETTYPE;
            var taskCurrentPeriod = _performanceRepository.QueryAsync<BettypeTopChangeDTO>(criteriaCurrentPeriod);

            var prevPeriodDtos = await taskPrevPeriod;
            var currentPeriodDtos = await taskCurrentPeriod;

            prevPeriodDtos = prevPeriodDtos.GroupTo(_ => _.Key, BettypeTopChangeDTOSelector);
            currentPeriodDtos = currentPeriodDtos.GroupTo(_ => _.Key, BettypeTopChangeDTOSelector);

            var result = _viewModelMapper.ToViewModel(prevPeriodDtos, currentPeriodDtos, _ => _.Key, _ => _.DisplayName);

            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetPlatformTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.Dimension = Dimension.PLATFORM;
            var taskPrevPeriod = _performanceRepository.QueryAsync<PlatformTopChangeDTO>(criteriaPrevPeriod);

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.Dimension = Dimension.PLATFORM;
            var taskCurrentPeriod = _performanceRepository.QueryAsync<PlatformTopChangeDTO>(criteriaCurrentPeriod);

            var result = _viewModelMapper.ToViewModel(await taskPrevPeriod, await taskCurrentPeriod, _ => _.PlatformId, _ => _.PlatformName);

            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetProductTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.Dimension = Dimension.PRODUCT;
            var taskPrevPeriod = _performanceRepository.QueryAsync<ProductTopChangeDTO>(criteriaPrevPeriod);

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.Dimension = Dimension.PRODUCT;
            var taskCurrentPeriod = _performanceRepository.QueryAsync<ProductTopChangeDTO>(criteriaCurrentPeriod);

            var result = _viewModelMapper.ToViewModel(await taskPrevPeriod, await taskCurrentPeriod, _ => _.ProductId, _ => _.ProductName);

            return result;
        }

        private static Func<IGrouping<string, BettypeTopChangeDTO>, BettypeTopChangeDTO> BettypeTopChangeDTOSelector = _ =>
        {
            var first = _.First();
            return new BettypeTopChangeDTO
            {
                BetTypeDisplayID = first.BetTypeDisplayID,
                BetTypeDisplayName = first.BetTypeDisplayName,
                ProductId = first.ProductId,
                ProductName = first.ProductName,
                Total = _.Sum(__ => __.Total)
            };
        };
    }
}