using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Mappers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
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
            IPlatformPerformanceVMMapper viewModelMapper,
            IHandleRawData handleRawData
            ) : base(context, performanceRepository, handleRawData)
        {
            _viewModelMapper = viewModelMapper;
        }

        public async Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(PlatformPerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformDistributionCriteria, PlatformPerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformDistributionCriteria, PlatformPerformanceFilterMetaModel>(filter);

            var data = await _performanceRepository.QueryMultipleAsync<PlatformDistributionDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToDistributionVM(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1), filter.TopDistributions);

            return result;
        }

        public async Task<IEnumerable<MeasurementComparisonVM>> GetMeasurementDifferenceWithComparison(PlatformPerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformMeasurementComparisonCriteria, PlatformPerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformMeasurementComparisonCriteria, PlatformPerformanceFilterMetaModel>(filter);

            var data = await _performanceRepository.QueryMultipleAsync<PlatformMeasurementComparisonDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var (prevPeriodData, currentPeriodData) = await BindOthersData(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1), filter);

            var result = _viewModelMapper.ToMeasurementComparisonVM(prevPeriodData, currentPeriodData, filter.TopDistributions);

            return result;
        }

        public async Task<IEnumerable<MeasurementPeriodComparisonGrowthVM>> GetPeriodGrowthWithComparison(PlatformPerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformGrowthCriteria, PlatformPerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformGrowthCriteria, PlatformPerformanceFilterMetaModel>(filter);

            var data = await _performanceRepository.QueryMultipleAsync<PlatformMeasurementPeriodDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var (prevPeriodData, currentPeriodData) = await BindOthersData(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1), filter);

            HandleRawData(prevPeriodData, filter);
            HandleRawData(currentPeriodData, filter);

            var result = _viewModelMapper.ToMeasurementGrowthPeriodVM(prevPeriodData, currentPeriodData, filter.TopDistributions);

            return result;
        }

        public async Task<IDictionary<string, object>> GetAllDetailMonthGrowthWithComparison(PlatformPerformanceFilterMetaModel filter)
        {
            var result = new Dictionary<string, object>(filter.TopDistributions.Count());

            var cloneFilter = MetaModelHelper.Clone(filter);
            cloneFilter = MetaModelHelper.BackToPeriodByMonth(cloneFilter, 1);
            cloneFilter = MetaModelHelper.BackToPeriodByMonth(cloneFilter, 1, false);

            var (prevPeriodFromDate, prevPeriodToDate) = (
               DateTimeHelper.ParseFromFormatDateToDate(filter.PreviousPeriod.FromMonth),
               DateTimeHelper.ParseFromFormatDateToDate(filter.PreviousPeriod.ToMonth)
               );

            var (currentPeriodFromDate, currentPeriodToDate) = (
                DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.FromMonth),
                DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.ToMonth)
                );

            foreach (var topDistribution in filter.TopDistributions)
            {
                cloneFilter.Platforms = topDistribution.PreviousPeriodDistributions?.Select(_ => _.Id);
                var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformGrowthCriteria, PlatformPerformanceFilterMetaModel>(cloneFilter, false);
                criteriaPrevPeriod.GroupBy = GroupByConst.MONTH_GROUP;

                cloneFilter.Platforms = topDistribution.CurrentPeriodDistributions.Select(_ => _.Id);
                var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformGrowthCriteria, PlatformPerformanceFilterMetaModel>(cloneFilter);
                criteriaCurrentPeriod.GroupBy = GroupByConst.MONTH_GROUP;

                var data = await _performanceRepository.QueryMultipleAsync<MeasurementTimeDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);
                HandleRawData(data, filter);

                var prevPeriodResult = _viewModelMapper.ToMeasurementGrowthTimeVM(data.ElementAtOrDefault(0), prevPeriodFromDate, prevPeriodToDate);
                var currentPeriodResult = _viewModelMapper.ToMeasurementGrowthTimeVM(data.ElementAtOrDefault(1), currentPeriodFromDate, currentPeriodToDate);

                result.Add(GetFieldNameOfMonthGrowthObject(topDistribution.DisplayId, filter.PreviousPeriod), prevPeriodResult);
                result.Add(GetFieldNameOfMonthGrowthObject(topDistribution.DisplayId, filter.CurrentPeriod), currentPeriodResult);
            }

            return result;
        }

        public async Task<IEnumerable<OverallMeasurementVM>> GetOverallMeasurement(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<PlatformDistributionCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<PlatformDistributionCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;

            var data = await _performanceRepository.QueryMultipleAsync<TotalDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);
            HandleRawData(data, filter);

            var result = _viewModelMapper.ToOverallMeasurementVM(data.ElementAtOrDefault(0)?.FirstOrDefault(), data.ElementAtOrDefault(1)?.FirstOrDefault());

            return result;
        }

        public async Task<(IEnumerable<DistributionDTO> prevPeriodData, IEnumerable<DistributionDTO> currentPeriodData)> GetDistributionRawData(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<PlatformDistributionCriteria, PerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<PlatformDistributionCriteria, PerformanceFilterMetaModel>(filter);

            var data = await _performanceRepository.QueryMultipleAsync<PlatformDistributionDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            return (data.ElementAtOrDefault(0), data.ElementAtOrDefault(1));
        }

        protected override async Task<IEnumerable<DistributionDTO>> GetDistributionData(PerformanceFilterMetaModel filter)
        {
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<PlatformDistributionCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.SortByMeasurement = SortByMeasurementConst.SORT;
            var distributionData = (await _performanceRepository.QueryAsync<PlatformDistributionDTO>(criteriaCurrentPeriod));

            var specialDistribution = distributionData.FirstOrDefault(_ => _.Id == PLATFORM_SPECIAL_ID);
            if (specialDistribution != null && distributionData.Count() > NUMBER_OF_TOP_DISTRIBUTION)
            {
                specialDistribution.Distribution = 0;
            }
            return distributionData;
        }

        private async Task<(IEnumerable<PlatformMeasurementPeriodDTO> prevPeriodData, IEnumerable<PlatformMeasurementPeriodDTO> currentPeriodData)> BindOthersData(
            IEnumerable<PlatformMeasurementPeriodDTO> prevPeriodData,
            IEnumerable<PlatformMeasurementPeriodDTO> currentPeriodData,
            PlatformPerformanceFilterMetaModel filter)
        {
            var otherDistribution = filter.TopDistributions.FirstOrDefault(_ => _.DisplayId == ViewModelMapper.DISTRIBUTION_OTHERS_ID);
            if (otherDistribution != null)
            {
                var cloneFilter = MetaModelHelper.Clone(filter);
                cloneFilter.Platforms = otherDistribution.PreviousPeriodDistributions?.Select(_ => _.Id);
                var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformGrowthCriteria, PlatformPerformanceFilterMetaModel>(cloneFilter, false);
                criteriaPrevPeriod.GroupBy = GroupByConst.NONE_GROUP;

                cloneFilter.Platforms = otherDistribution.CurrentPeriodDistributions.Select(_ => _.Id);
                var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformGrowthCriteria, PlatformPerformanceFilterMetaModel>(cloneFilter);
                criteriaCurrentPeriod.GroupBy = GroupByConst.NONE_GROUP;

                var othersData = await _performanceRepository.QueryMultipleAsync<PlatformMeasurementPeriodDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

                var prevPeriodOthersData = othersData.ElementAt(0).FirstOrDefault() ?? CreationalHelper.Create<PlatformMeasurementPeriodDTO>();
                prevPeriodOthersData.PlatformId = ViewModelMapper.DISTRIBUTION_OTHERS_ID;

                var currentPeriodOthersData = othersData.ElementAt(1).First();
                currentPeriodOthersData.PlatformId = ViewModelMapper.DISTRIBUTION_OTHERS_ID;

                prevPeriodData = prevPeriodData.Append(prevPeriodOthersData);
                currentPeriodData = currentPeriodData.Append(currentPeriodOthersData);
            }

            return (prevPeriodData, currentPeriodData);
        }

        private async Task<(IEnumerable<PlatformMeasurementComparisonDTO> prevPeriodData, IEnumerable<PlatformMeasurementComparisonDTO> currentPeriodData)> BindOthersData(
            IEnumerable<PlatformMeasurementComparisonDTO> prevPeriodData,
            IEnumerable<PlatformMeasurementComparisonDTO> currentPeriodData,
            PlatformPerformanceFilterMetaModel filter)
        {
            var otherDistribution = filter.TopDistributions.FirstOrDefault(_ => _.DisplayId == ViewModelMapper.DISTRIBUTION_OTHERS_ID);
            if (otherDistribution != null)
            {
                var cloneFilter = MetaModelHelper.Clone(filter);

                cloneFilter.Platforms = otherDistribution.PreviousPeriodDistributions?.Select(_ => _.Id);
                var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformMeasurementComparisonCriteria, PlatformPerformanceFilterMetaModel>(cloneFilter, false);
                criteriaPrevPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;
                criteriaPrevPeriod.SortByMeasurement = SortByMeasurementConst.SORT;

                cloneFilter.Platforms = otherDistribution.CurrentPeriodDistributions.Select(_ => _.Id);
                var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformMeasurementComparisonCriteria, PlatformPerformanceFilterMetaModel>(cloneFilter);
                criteriaCurrentPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;
                criteriaCurrentPeriod.SortByMeasurement = SortByMeasurementConst.SORT;

                var othersData = await _performanceRepository.QueryMultipleAsync<PlatformMeasurementComparisonDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

                var prevPeriodOthersData = othersData.ElementAt(0).FirstOrDefault() ?? CreationalHelper.Create<PlatformMeasurementComparisonDTO>();
                prevPeriodOthersData.PlatformId = ViewModelMapper.DISTRIBUTION_OTHERS_ID;

                var currentPeriodOthersData = othersData.ElementAt(1).First();
                currentPeriodOthersData.PlatformId = ViewModelMapper.DISTRIBUTION_OTHERS_ID;

                prevPeriodData = prevPeriodData.Append(prevPeriodOthersData);
                currentPeriodData = currentPeriodData.Append(currentPeriodOthersData);
            }

            return (prevPeriodData, currentPeriodData);
        }
    }
}