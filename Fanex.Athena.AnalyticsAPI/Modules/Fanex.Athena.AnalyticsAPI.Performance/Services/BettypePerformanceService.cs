using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Mappers;
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
            IBettypePerformanceVMMapper viewModelMapper,
            IHandleRawData handleRawData
            ) : base(context, performanceRepository, handleRawData)
        {
            _viewModelMapper = viewModelMapper;
        }

        public async Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(BettypePerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeDistributionCriteria, BettypePerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeDistributionCriteria, BettypePerformanceFilterMetaModel>(filter);

            var data = await _performanceRepository.QueryMultipleAsync<BettypeDistributionDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToDistributionVM(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1), filter.TopDistributions);

            return result;
        }

        public async Task<IEnumerable<MeasurementComparisonVM>> GetMeasurementDifferenceWithComparison(BettypePerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeMeasurementComparisonCriteria, BettypePerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeMeasurementComparisonCriteria, BettypePerformanceFilterMetaModel>(filter);

            var data = await _performanceRepository.QueryMultipleAsync<BettypeMeasurementComparisonDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var (prevPeriodData, currentPeriodData) = await BindOthersData(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1), filter);

            var result = _viewModelMapper.ToMeasurementComparisonVM(prevPeriodData, currentPeriodData, filter.TopDistributions);

            return result;
        }

        public async Task<IEnumerable<MeasurementPeriodComparisonGrowthVM>> GetPeriodGrowthWithComparison(BettypePerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeGrowthCriteria, BettypePerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeGrowthCriteria, BettypePerformanceFilterMetaModel>(filter);

            var data = await _performanceRepository.QueryMultipleAsync<BettypeMeasurementPeriodDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var (prevPeriodData, currentPeriodData) = await BindOthersData(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1), filter);

            HandleRawData(prevPeriodData, filter);
            HandleRawData(currentPeriodData, filter);

            var result = _viewModelMapper.ToMeasurementGrowthPeriodVM(prevPeriodData, currentPeriodData, filter.TopDistributions);

            return result;
        }

        public async Task<IDictionary<string, object>> GetAllDetailMonthGrowthWithComparison(BettypePerformanceFilterMetaModel filter)
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
                cloneFilter.CombineIds = topDistribution.PreviousPeriodDistributions?.Select(_ => _.Id);
                var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeGrowthCriteria, BettypePerformanceFilterMetaModel>(cloneFilter, false);
                criteriaPrevPeriod.GroupBy = GroupByConst.MONTH_GROUP;

                cloneFilter.CombineIds = topDistribution.CurrentPeriodDistributions.Select(_ => _.Id);
                var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeGrowthCriteria, BettypePerformanceFilterMetaModel>(cloneFilter);
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
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<BettypeDistributionCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<BettypeDistributionCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;

            var data = await _performanceRepository.QueryMultipleAsync<TotalDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);
            HandleRawData(data, filter);

            var result = _viewModelMapper.ToOverallMeasurementVM(data.ElementAtOrDefault(0)?.FirstOrDefault(), data.ElementAtOrDefault(1)?.FirstOrDefault());

            return result;
        }

        public async Task<(IEnumerable<DistributionDTO> prevPeriodData, IEnumerable<DistributionDTO> currentPeriodData)> GetDistributionRawData(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<BettypeDistributionCriteria, PerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<BettypeDistributionCriteria, PerformanceFilterMetaModel>(filter);

            var data = await _performanceRepository.QueryMultipleAsync<BettypeDistributionDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            return (data.ElementAtOrDefault(0), data.ElementAtOrDefault(1));
        }

        protected override async Task<IEnumerable<DistributionDTO>> GetDistributionData(PerformanceFilterMetaModel filter)
        {
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<BettypeDistributionCriteria, PerformanceFilterMetaModel>(filter);
            var distributionData = await _performanceRepository.QueryAsync<BettypeDistributionDTO>(criteriaCurrentPeriod);

            return distributionData;
        }

        private async Task<(IEnumerable<BettypeMeasurementPeriodDTO> prevPeriodData, IEnumerable<BettypeMeasurementPeriodDTO> currentPeriodData)> BindOthersData(
            IEnumerable<BettypeMeasurementPeriodDTO> prevPeriodData,
            IEnumerable<BettypeMeasurementPeriodDTO> currentPeriodData,
            BettypePerformanceFilterMetaModel filter)
        {
            var otherDistribution = filter.TopDistributions.FirstOrDefault(_ => _.DisplayId == ViewModelMapper.DISTRIBUTION_OTHERS_ID);
            if (otherDistribution != null)
            {
                var cloneFilter = MetaModelHelper.Clone(filter);
                cloneFilter.CombineIds = otherDistribution.PreviousPeriodDistributions?.Select(_ => _.Id);
                var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeGrowthCriteria, BettypePerformanceFilterMetaModel>(cloneFilter, false);
                criteriaPrevPeriod.GroupBy = GroupByConst.NONE_GROUP;

                cloneFilter.CombineIds = otherDistribution.CurrentPeriodDistributions.Select(_ => _.Id);
                var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeGrowthCriteria, BettypePerformanceFilterMetaModel>(cloneFilter);
                criteriaCurrentPeriod.GroupBy = GroupByConst.NONE_GROUP;

                var othersData = await _performanceRepository.QueryMultipleAsync<BettypeMeasurementPeriodDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);
                var prevPeriodOthersData = othersData.ElementAtOrDefault(0)?.FirstOrDefault() ?? CreationalHelper.Create<BettypeMeasurementPeriodDTO>();
                prevPeriodOthersData.BetTypeDisplayID = ViewModelMapper.DISTRIBUTION_OTHERS_ID;

                var currentPeriodOthersData = othersData.ElementAtOrDefault(1)?.FirstOrDefault();
                currentPeriodOthersData.BetTypeDisplayID = ViewModelMapper.DISTRIBUTION_OTHERS_ID;

                prevPeriodData = prevPeriodOthersData != null ? prevPeriodData.Append(prevPeriodOthersData) : prevPeriodData;
                currentPeriodData = currentPeriodOthersData != null ? currentPeriodData.Append(currentPeriodOthersData) : currentPeriodData;
            }

            return (prevPeriodData, currentPeriodData);
        }

        private async Task<(IEnumerable<BettypeMeasurementComparisonDTO> prevPeriodData, IEnumerable<BettypeMeasurementComparisonDTO> currentPeriodData)> BindOthersData(
            IEnumerable<BettypeMeasurementComparisonDTO> prevPeriodData,
            IEnumerable<BettypeMeasurementComparisonDTO> currentPeriodData,
            BettypePerformanceFilterMetaModel filter)
        {
            var otherDistribution = filter.TopDistributions.FirstOrDefault(_ => _.DisplayId == ViewModelMapper.DISTRIBUTION_OTHERS_ID);
            if (otherDistribution != null)
            {
                var cloneFilter = MetaModelHelper.Clone(filter);

                cloneFilter.CombineIds = otherDistribution.PreviousPeriodDistributions?.Select(_ => _.Id);
                var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeMeasurementComparisonCriteria, BettypePerformanceFilterMetaModel>(cloneFilter, false);
                criteriaPrevPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;
                criteriaPrevPeriod.SortByMeasurement = SortByMeasurementConst.SORT;

                cloneFilter.CombineIds = otherDistribution.CurrentPeriodDistributions.Select(_ => _.Id);
                var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToBettypePerformanceCriteria<BettypeMeasurementComparisonCriteria, BettypePerformanceFilterMetaModel>(cloneFilter);
                criteriaCurrentPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;
                criteriaCurrentPeriod.SortByMeasurement = SortByMeasurementConst.SORT;

                var othersData = await _performanceRepository.QueryMultipleAsync<BettypeMeasurementComparisonDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

                var prevPeriodOthersData = othersData.ElementAtOrDefault(0)?.FirstOrDefault() ?? CreationalHelper.Create<BettypeMeasurementComparisonDTO>();
                prevPeriodOthersData.BetTypeDisplayID = ViewModelMapper.DISTRIBUTION_OTHERS_ID;

                var currentPeriodOthersData = othersData.ElementAtOrDefault(1)?.FirstOrDefault();
                currentPeriodOthersData.BetTypeDisplayID = ViewModelMapper.DISTRIBUTION_OTHERS_ID;

                prevPeriodData = prevPeriodOthersData != null ? prevPeriodData.Append(prevPeriodOthersData) : prevPeriodData;
                currentPeriodData = currentPeriodOthersData != null ? currentPeriodData.Append(currentPeriodOthersData) : currentPeriodData;
            }

            return (prevPeriodData, currentPeriodData);
        }
    }
}