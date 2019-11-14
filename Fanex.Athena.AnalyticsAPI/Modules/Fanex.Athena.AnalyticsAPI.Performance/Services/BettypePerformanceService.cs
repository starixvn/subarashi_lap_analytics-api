using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerformanceHelper = Fanex.Athena.AnalyticsAPI.Performance.Helpers;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class BettypePerformanceService : PerformanceService, IBettypePerformanceService
    {
        private readonly IBettypePerformanceVMMapper _viewModelMapper;

        public BettypePerformanceService(
            IContextService context,
            IPerformanceRepository performanceRepository,
            IBettypePerformanceVMMapper viewModelMapper
            ) : base(context, performanceRepository)
        {
            _viewModelMapper = viewModelMapper;
        }

        public async Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(BettypePerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeDistributionCriteria, BettypePerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeDistributionCriteria, BettypePerformanceFilterMetaModel>(filter);

            var dtos = await _performanceRepository.QueryMultipleAsync<BettypeDistributionDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToDistributionVM(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1), MapToTopDistributions(filter.TopDistributions));

            return result;
        }

        public async Task<IEnumerable<MeasurementPeriodComparisonGrowthVM>> GetPeriodGrowthWithComparison(BettypePerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeGrowthCriteria, BettypePerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeGrowthCriteria, BettypePerformanceFilterMetaModel>(filter);

            var dtos = await _performanceRepository.QueryMultipleAsync<BettypeMeasurementPeriodDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToMeasurementGrowthPeriodVM(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1), MapToTopDistributions(filter.TopDistributions));

            return result;
        }

        public async Task<IEnumerable<MeasurementGrowthVM>> GetDetailMonthGrowthWithComparison(BettypePerformanceFilterMetaModel filter)
        {
            var newFilter = MetaModelHelper.BackToPeriodByMonth(filter, 1);

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeGrowthCriteria, BettypePerformanceFilterMetaModel>(newFilter);
            criteriaCurrentPeriod.GroupBy = GroupByConst.MONTH_GROUP;
            var currentPeriodData = await _performanceRepository.QueryAsync<MeasurementTimeDTO>(criteriaCurrentPeriod);

            var result = _viewModelMapper.ToMeasurementGrowthTimeVM(currentPeriodData,
                DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.FromMonth),
                DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.ToMonth)
                );

            return result;
        }

        public async Task<IDictionary<string, object>> GetAllDetailMonthGrowthWithComparison(BettypePerformanceFilterMetaModel filter)
        {
            var result = new Dictionary<string, object>(filter.TopDistributions.Count());

            var cloneFilter = MetaModelHelper.Clone(filter);
            cloneFilter = MetaModelHelper.BackToPeriodByMonth(cloneFilter, 1);
            cloneFilter = MetaModelHelper.BackToPeriodByMonth(cloneFilter, 1, false);

            foreach (var topDistribution in filter.TopDistributions)
            {
                cloneFilter.CombineIds = topDistribution.PreviousPeriodDistributions.Select(_ => _.Id);
                var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeGrowthCriteria, BettypePerformanceFilterMetaModel>(cloneFilter, false);
                criteriaPrevPeriod.GroupBy = GroupByConst.MONTH_GROUP;

                cloneFilter.CombineIds = topDistribution.CurrentPeriodDistributions.Select(_ => _.Id);
                var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeGrowthCriteria, BettypePerformanceFilterMetaModel>(cloneFilter);
                criteriaCurrentPeriod.GroupBy = GroupByConst.MONTH_GROUP;

                var dtos = await _performanceRepository.QueryMultipleAsync<MeasurementTimeDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

                var prevPeriodResult = _viewModelMapper.ToMeasurementGrowthTimeVM(dtos.ElementAtOrDefault(0),
                        DateTimeHelper.ParseFromFormatDateToDate(filter.PreviousPeriod.FromMonth),
                        DateTimeHelper.ParseFromFormatDateToDate(filter.PreviousPeriod.ToMonth)
                        );

                var currentPeriodResult = _viewModelMapper.ToMeasurementGrowthTimeVM(dtos.ElementAtOrDefault(1),
                       DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.FromMonth),
                       DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.ToMonth)
                       );

                result.Add(GetFieldNameOfMonthGrowthObject(topDistribution.DisplayId, filter.PreviousPeriod), prevPeriodResult);
                result.Add(GetFieldNameOfMonthGrowthObject(topDistribution.DisplayId, filter.CurrentPeriod), currentPeriodResult);
            }

            return result;
        }

        public async Task<IEnumerable<MeasurementComparisonVM>> GetMeasurementDifferenceWithComparison(BettypePerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeMeasurementComparisonCriteria, BettypePerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeMeasurementComparisonCriteria, BettypePerformanceFilterMetaModel>(filter);

            var dtos = await _performanceRepository.QueryMultipleAsync<BettypeMeasurementComparisonDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToMeasurementComparisonVMV2(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1), MapToTopDistributions(filter.TopDistributions));

            return result;
        }

        public async Task<IEnumerable<MeasurementGrowthVM>> GetDetailMonthGrowthWithComparison(PlatformPerformanceFilterMetaModel filter)
        {
            var newFilter = MetaModelHelper.BackToPeriodByMonth(filter, 1);

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformGrowthCriteria, PlatformPerformanceFilterMetaModel>(newFilter);
            criteriaCurrentPeriod.GroupBy = GroupByConst.MONTH_GROUP;
            var currentPeriodData = await _performanceRepository.QueryAsync<MeasurementTimeDTO>(criteriaCurrentPeriod);

            var result = _viewModelMapper.ToMeasurementGrowthTimeVM(currentPeriodData,
                DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.FromMonth),
                DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.ToMonth)
                );

            return result;
        }

        public async Task<IEnumerable<MeasurementComparisonVM>> GetMeasurementDifferenceWithComparison(PlatformPerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformMeasurementComparisonCriteria, PlatformPerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformMeasurementComparisonCriteria, PlatformPerformanceFilterMetaModel>(filter);

            var dtos = await _performanceRepository.QueryMultipleAsync<PlatformMeasurementComparisonDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);
            var result = _viewModelMapper.ToMeasurementComparisonVMV2(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1), MapToTopDistributions(filter.TopDistributions));

            return result;
        }

        public async Task<IEnumerable<OverallMeasurementVM>> GetOverallMeasurement(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<BettypeDistributionCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<BettypeDistributionCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;

            var dtos = await _performanceRepository.QueryMultipleAsync<TotalDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToOverallMeasurement(dtos.ElementAtOrDefault(0)?.FirstOrDefault(), dtos.ElementAtOrDefault(1)?.FirstOrDefault());

            return result;
        }

        protected override async Task<IEnumerable<DistributionDTO>> GetDistributionData(PerformanceFilterMetaModel filter)
        {
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<BettypeDistributionCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.SortByMeasurement = SortByMeasurementConst.SORT;
            var distributionData = await _performanceRepository.QueryAsync<BettypeDistributionDTO>(criteriaCurrentPeriod);

            return distributionData;
        }
    }
}