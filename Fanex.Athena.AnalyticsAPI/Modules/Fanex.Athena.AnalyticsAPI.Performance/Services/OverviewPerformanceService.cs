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
            var criteriaPrevOfPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewMeasurementGrowthCriteria, PerformanceFilterMetaModel>(filter);
            CriteriaHelper.UpdateCurrentPeriodToPreviousPeriod(criteriaPrevOfPrevPeriod, filter.PreviousPeriod, filter.CurrentPeriod);
            criteriaPrevOfPrevPeriod.GroupID = GroupDataConst.NONE_GROUP;

            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewMeasurementGrowthCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.GroupID = GroupDataConst.NONE_GROUP;

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewMeasurementGrowthCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.GroupID = GroupDataConst.NONE_GROUP;

            var dtos = await _performanceRepository.QueryMultipleAsync<TotalDTO>(criteriaPrevOfPrevPeriod, criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToMeasurementGrowthPeriodVM(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1), dtos.ElementAtOrDefault(2));

            return result;
        }

        public async Task<IEnumerable<MeasurementGrowthVM>> GetDetailMonthGrowthWithComparison(PerformanceFilterMetaModel filter)
        {
            var newFilter = MetaModelHelper.BackToPeriodByMonth(filter, 1);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewMeasurementGrowthCriteria, PerformanceFilterMetaModel>(newFilter);
            criteriaCurrentPeriod.GroupID = GroupDataConst.MONTH_GROUP;
            var taskCurrentPeriod = _performanceRepository.QueryAsync<MeasurementTimeDTO>(criteriaCurrentPeriod);

            var result = _viewModelMapper.ToMeasurementGrowthTimeVM(
                await taskCurrentPeriod,
                DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.FromMonth),
                DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.ToMonth)
                );

            return result;
        }

        public async Task<IEnumerable<SummaryVM>> GetSummaryWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = CriteriaHelper.ToCriteria<OverviewSummaryCriteria, PerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = CommonHelper.CriteriaHelper.ToCriteria<OverviewSummaryCriteria, PerformanceFilterMetaModel>(filter);

            var dtos = await _performanceRepository.QueryMultipleAsync<SummaryDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToSummaryVM(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1));

            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetCurrencyTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.Dimension = DimensionConst.CURRENCY;

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.Dimension = DimensionConst.CURRENCY;

            var dtos = await _performanceRepository.QueryMultipleAsync<CurrencyTopChangeDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToTopChangeVM(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1), _ => _.CurrencyId, _ => _.CurrencyName);

            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetBettypeTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.Dimension = DimensionConst.BETTYPE;
            var taskPrevPeriod = _performanceRepository.QueryAsync<BettypeTopChangeDTO>(criteriaPrevPeriod);

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.Dimension = DimensionConst.BETTYPE;
            var taskCurrentPeriod = _performanceRepository.QueryAsync<BettypeTopChangeDTO>(criteriaCurrentPeriod);

            var prevPeriodDtos = await taskPrevPeriod;
            var currentPeriodDtos = await taskCurrentPeriod;

            prevPeriodDtos = prevPeriodDtos.GroupTo(_ => _.Key, BettypeTopChangeDTOSelector);
            currentPeriodDtos = currentPeriodDtos.GroupTo(_ => _.Key, BettypeTopChangeDTOSelector);

            var result = _viewModelMapper.ToTopChangeVM(prevPeriodDtos, currentPeriodDtos, _ => _.Key, _ => _.DisplayName);

            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetPlatformTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.Dimension = DimensionConst.PLATFORM;

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.Dimension = DimensionConst.PLATFORM;

            var dtos = await _performanceRepository.QueryMultipleAsync<PlatformTopChangeDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToTopChangeVM(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1), _ => _.PlatformId, _ => _.PlatformName);

            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetProductTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.Dimension = DimensionConst.PRODUCT;

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.Dimension = DimensionConst.PRODUCT;

            var dtos = await _performanceRepository.QueryMultipleAsync<ProductTopChangeDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToTopChangeVM(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1), _ => _.ProductId, _ => _.ProductName);

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