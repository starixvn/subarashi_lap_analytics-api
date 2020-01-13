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
            IOverviewPerformanceVMMapper viewModelMapper,
            IHandleRawData handleRawData                                               
            ) : base(context, performanceRepository, handleRawData)
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

            var data = await _performanceRepository.QueryMultipleAsync<TotalDTO>(criteriaPrevOfPrevPeriod, criteriaPrevPeriod, criteriaCurrentPeriod);
            HandleRawData(data, filter);

            var result = _viewModelMapper.ToMeasurementGrowthPeriodVM(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1), data.ElementAtOrDefault(2));

            return result;
        }

        public async Task<IEnumerable<MeasurementGrowthVM>> GetDetailMonthGrowthWithComparison(PerformanceFilterMetaModel filter)
        {
            var newFilter = MetaModelHelper.BackToPeriodByMonth(filter, 1);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewMeasurementGrowthCriteria, PerformanceFilterMetaModel>(newFilter);
            criteriaCurrentPeriod.GroupID = GroupDataConst.MONTH_GROUP;
            var data = await _performanceRepository.QueryAsync<MeasurementTimeDTO>(criteriaCurrentPeriod);
            HandleRawData(data, filter);

            var result = _viewModelMapper.ToMeasurementGrowthTimeVM(
                data,
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

            var data = await _performanceRepository.QueryMultipleAsync<CurrencyTopChangeDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);
            HandleRawData(data, filter);

            var result = _viewModelMapper.ToTopChangeVM(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1));

            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetBettypeTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.Dimension = DimensionConst.BETTYPE;

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.Dimension = DimensionConst.BETTYPE;

            var data = await _performanceRepository.QueryMultipleAsync<BettypeTopChangeDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var prevPeriodDtos = data.ElementAtOrDefault(0);
            var currentPeriodDtos = data.ElementAtOrDefault(1);

            prevPeriodDtos = prevPeriodDtos.GroupTo(_ => _.Id, bettypeTopChangeDTOSelector);
            currentPeriodDtos = currentPeriodDtos.GroupTo(_ => _.Id, bettypeTopChangeDTOSelector);

            HandleRawData(prevPeriodDtos, filter);
            HandleRawData(currentPeriodDtos, filter);

            var result = _viewModelMapper.ToTopChangeVM(prevPeriodDtos, currentPeriodDtos);

            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetPlatformTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.Dimension = DimensionConst.PLATFORM;

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.Dimension = DimensionConst.PLATFORM;

            var data = await _performanceRepository.QueryMultipleAsync<PlatformTopChangeDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);
            HandleRawData(data, filter);
            var result = _viewModelMapper.ToTopChangeVM(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1));

            return result;
        }

        public async Task<IEnumerable<TopChangeVM>> GetProductTopChangeWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.Dimension = DimensionConst.PRODUCT;

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<OverviewTopChangeCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.Dimension = DimensionConst.PRODUCT;

            var data = await _performanceRepository.QueryMultipleAsync<ProductTopChangeDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);
            HandleRawData(data, filter);
            var result = _viewModelMapper.ToTopChangeVM(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1));

            return result;
        }

        private static Func<IGrouping<string, BettypeTopChangeDTO>, BettypeTopChangeDTO> bettypeTopChangeDTOSelector = _ =>
        {
            var first = _.First();
            return new BettypeTopChangeDTO
            {
                BetTypeDisplayID = first.BetTypeDisplayID,
                BetTypeDisplayName = first.BetTypeDisplayName,
                ProductId = first.ProductId,
                ProductCode = first.ProductCode,
                Total = _.Sum(__ => __.Total)
            };
        };
    }
}