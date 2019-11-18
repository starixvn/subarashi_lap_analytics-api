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
    public class PlatformPerformanceService : PerformanceService, IPlatformPerformanceService
    {
        private const string PLATFORM_SPECIAL_ID = "-1";

        private readonly IPlatformPerformanceVMMapper _viewModelMapper;

        public PlatformPerformanceService(
            IContextService context,
            IPerformanceRepository performanceRepository,
            IPlatformPerformanceVMMapper viewModelMapper
            ) : base(context, performanceRepository)
        {
            _viewModelMapper = viewModelMapper;
        }

        public async Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(PlatformPerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformDistributionCriteria, PlatformPerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformDistributionCriteria, PlatformPerformanceFilterMetaModel>(filter);

            var dtos = await _performanceRepository.QueryMultipleAsync<PlatformDistributionDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToDistributionVM(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1), MapToTopDistributions(filter.TopDistributions));

            return result;
        }

        public async Task<IEnumerable<MeasurementPeriodComparisonGrowthVM>> GetPeriodGrowthWithComparison(PlatformPerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformGrowthCriteria, PlatformPerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformGrowthCriteria, PlatformPerformanceFilterMetaModel>(filter);

            var dtos = await _performanceRepository.QueryMultipleAsync<PlatformMeasurementPeriodDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToMeasurementGrowthPeriodVM(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1), MapToTopDistributions(filter.TopDistributions));

            return result;
        }

        public async Task<IDictionary<string, object>> GetAllDetailMonthGrowthWithComparison(PlatformPerformanceFilterMetaModel filter)
        {
            var result = new Dictionary<string, object>(filter.TopDistributions.Count());

            var cloneFilter = MetaModelHelper.Clone(filter);
            cloneFilter = MetaModelHelper.BackToPeriodByMonth(cloneFilter, 1);
            cloneFilter = MetaModelHelper.BackToPeriodByMonth(cloneFilter, 1, false);

            foreach (var topDistribution in filter.TopDistributions)
            {
                cloneFilter.Platforms = topDistribution.PreviousPeriodDistributions.Select(_ => _.Id);
                var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformGrowthCriteria, PlatformPerformanceFilterMetaModel>(cloneFilter, false);
                criteriaPrevPeriod.GroupBy = GroupByConst.MONTH_GROUP;

                cloneFilter.Platforms = topDistribution.CurrentPeriodDistributions.Select(_ => _.Id);
                var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformGrowthCriteria, PlatformPerformanceFilterMetaModel>(cloneFilter);
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
            var result = _viewModelMapper.ToMeasurementComparisonVM(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1), MapToTopDistributions(filter.TopDistributions));

            return result;
        }

        public async Task<IEnumerable<OverallMeasurementVM>> GetOverallMeasurement(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<ProductDistributionCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<ProductDistributionCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;

            var dtos = await _performanceRepository.QueryMultipleAsync<TotalDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToOverallMeasurement(dtos.ElementAtOrDefault(0)?.FirstOrDefault(), dtos.ElementAtOrDefault(1)?.FirstOrDefault());

            return result;
        }

        protected override async Task<IEnumerable<DistributionDTO>> GetDistributionData(PerformanceFilterMetaModel filter)
        {
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<PlatformDistributionCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.SortByMeasurement = SortByMeasurementConst.SORT;
            var distributionData = await _performanceRepository.QueryAsync<PlatformDistributionDTO>(criteriaCurrentPeriod);

            PlatformDistributionDTO specialDistribution = null;
            if ((specialDistribution = distributionData.FirstOrDefault(_ => _.Id == PLATFORM_SPECIAL_ID)) != null
                && distributionData.Count() > NUMBER_OF_TOP_DISTRIBUTION)
            {
                specialDistribution.Distribution = -1;
            }
            return distributionData;
        }
    }
}