using PerformanceHelper = Fanex.Athena.AnalyticsAPI.Performance.Helpers;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Helpers;
    using Common.Mappers;
    using Common.Services;
    using Mappers;
    using Models.Criteria;
    using Models.DTO;
    using Models.MetaModels;
    using Models.ViewModels;
    using Repositories;

    public class CurrencyPerformanceService : PerformanceService, ICurrencyPerformanceService
    {
        private readonly ICurrencyPerformanceVMMapper _viewModelMapper;

        public CurrencyPerformanceService(
            IContextService context,
            IPerformanceRepository performanceRepository,
            ICurrencyPerformanceVMMapper viewModelMapper,
            IHandleRawData handleRawData
            ) : base(context, performanceRepository, handleRawData)
        {
            _viewModelMapper = viewModelMapper;
        }

        public async Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(CurrenyPerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, CurrenyPerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, CurrenyPerformanceFilterMetaModel>(filter);

            var data = await _performanceRepository.QueryMultipleAsync<CurrencyDistributionDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToDistributionVM(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1), filter.TopDistributions);

            return result;
        }

        public async Task<IEnumerable<MeasurementComparisonVM>> GetMeasurementDifferenceWithComparison(CurrenyPerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyMeasurementComparisonCriteria, CurrenyPerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyMeasurementComparisonCriteria, CurrenyPerformanceFilterMetaModel>(filter);

            var data = await _performanceRepository.QueryMultipleAsync<CurrencyMeasurementComparisonDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var (prevPeriodData, currentPeriodData) = await BindOthersData(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1), filter);

            var result = _viewModelMapper.ToMeasurementComparisonVM(prevPeriodData, currentPeriodData, filter.TopDistributions);

            return result;
        }

        public async Task<IEnumerable<MeasurementPeriodComparisonGrowthVM>> GetPeriodGrowthWithComparison(CurrenyPerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyGrowthCriteria, CurrenyPerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyGrowthCriteria, CurrenyPerformanceFilterMetaModel>(filter);

            var data = await _performanceRepository.QueryMultipleAsync<CurrencyMeasurementPeriodDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var (prevPeriodData, currentPeriodData) = await BindOthersData(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1), filter);

            HandleRawData(prevPeriodData, filter);
            HandleRawData(currentPeriodData, filter);

            var result = _viewModelMapper.ToMeasurementGrowthPeriodVM(prevPeriodData, currentPeriodData, filter.TopDistributions);

            return result;
        }

        public async Task<IDictionary<string, object>> GetAllDetailMonthGrowthWithComparison(CurrenyPerformanceFilterMetaModel filter)
        {
            var result = new Dictionary<string, object>(filter.TopDistributions.Count() * 2);

            var cloneFilter = filter.Clone();
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
                cloneFilter.Currencies = topDistribution.PreviousPeriodDistributions?.Select(_ => _.Name);
                var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyGrowthCriteria, CurrenyPerformanceFilterMetaModel>(cloneFilter, false);
                criteriaPrevPeriod.GroupBy = GroupByConst.MONTH_GROUP;

                cloneFilter.Currencies = topDistribution.CurrentPeriodDistributions.Select(_ => _.Name);
                var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyGrowthCriteria, CurrenyPerformanceFilterMetaModel>(cloneFilter);
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
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;

            var data = await _performanceRepository.QueryMultipleAsync<TotalDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            HandleRawData(data, filter);

            var result = _viewModelMapper.ToOverallMeasurementVM(data.ElementAtOrDefault(0)?.FirstOrDefault(), data.ElementAtOrDefault(1)?.FirstOrDefault());

            return result;
        }

        public async Task<(IEnumerable<DistributionDTO> prevPeriodData, IEnumerable<DistributionDTO> currentPeriodData)> GetDistributionRawData(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, PerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, PerformanceFilterMetaModel>(filter);

            var data = await _performanceRepository.QueryMultipleAsync<CurrencyDistributionDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            return (data.ElementAtOrDefault(0), data.ElementAtOrDefault(1));
        }

        protected override async Task<IEnumerable<DistributionDTO>> GetDistributionData(PerformanceFilterMetaModel filter)
        {
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, PerformanceFilterMetaModel>(filter);
            var distributionData = await _performanceRepository.QueryAsync<CurrencyDistributionDTO>(criteriaCurrentPeriod);

            return distributionData;
        }

        private async Task<(IEnumerable<CurrencyMeasurementPeriodDTO> prevPeriodData, IEnumerable<CurrencyMeasurementPeriodDTO> currentPeriodData)> BindOthersData(
            IEnumerable<CurrencyMeasurementPeriodDTO> prevPeriodData,
            IEnumerable<CurrencyMeasurementPeriodDTO> currentPeriodData,
            CurrenyPerformanceFilterMetaModel filter)
        {
            var otherDistribution = filter.TopDistributions.FirstOrDefault(_ => _.DisplayId == ViewModelMapper.DISTRIBUTION_OTHERS_ID);
            if (otherDistribution != null)
            {
                var cloneFilter = filter.Clone();
                cloneFilter.Currencies = otherDistribution.PreviousPeriodDistributions?.Select(_ => _.Name);
                var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyGrowthCriteria, CurrenyPerformanceFilterMetaModel>(cloneFilter, false);
                criteriaPrevPeriod.GroupBy = GroupByConst.NONE_GROUP;

                cloneFilter.Currencies = otherDistribution.CurrentPeriodDistributions.Select(_ => _.Name);
                var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyGrowthCriteria, CurrenyPerformanceFilterMetaModel>(cloneFilter);
                criteriaCurrentPeriod.GroupBy = GroupByConst.NONE_GROUP;

                var othersData = await _performanceRepository.QueryMultipleAsync<CurrencyMeasurementPeriodDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);
                var prevPeriodOthersData = othersData.ElementAt(0).FirstOrDefault() ?? CreationalHelper.Create<CurrencyMeasurementPeriodDTO>();
                prevPeriodOthersData.CurrencyId = ViewModelMapper.DISTRIBUTION_OTHERS_ID;

                var currentPeriodOthersData = othersData.ElementAt(1).First();
                currentPeriodOthersData.CurrencyId = ViewModelMapper.DISTRIBUTION_OTHERS_ID;

                prevPeriodData = prevPeriodData.Append(prevPeriodOthersData);
                currentPeriodData = currentPeriodData.Append(currentPeriodOthersData);
            }

            return (prevPeriodData, currentPeriodData);
        }

        private async Task<(IEnumerable<CurrencyMeasurementComparisonDTO> prevPeriodData, IEnumerable<CurrencyMeasurementComparisonDTO> currentPeriodData)> BindOthersData(
            IEnumerable<CurrencyMeasurementComparisonDTO> prevPeriodData,
            IEnumerable<CurrencyMeasurementComparisonDTO> currentPeriodData,
            CurrenyPerformanceFilterMetaModel filter)
        {
            var otherDistribution = filter.TopDistributions.FirstOrDefault(_ => _.DisplayId == ViewModelMapper.DISTRIBUTION_OTHERS_ID);
            if (otherDistribution != null)
            {
                var cloneFilter = filter.Clone();

                cloneFilter.Currencies = otherDistribution.PreviousPeriodDistributions?.Select(_ => _.Name);
                var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyMeasurementComparisonCriteria, CurrenyPerformanceFilterMetaModel>(cloneFilter, false);
                criteriaPrevPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;
                criteriaPrevPeriod.SortByMeasurement = SortByMeasurementConst.SORT;

                cloneFilter.Currencies = otherDistribution.CurrentPeriodDistributions.Select(_ => _.Name);
                var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyMeasurementComparisonCriteria, CurrenyPerformanceFilterMetaModel>(cloneFilter);
                criteriaCurrentPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;
                criteriaCurrentPeriod.SortByMeasurement = SortByMeasurementConst.SORT;

                var othersData = await _performanceRepository.QueryMultipleAsync<CurrencyMeasurementComparisonDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

                var prevPeriodOthersData = othersData.ElementAt(0).FirstOrDefault() ?? CreationalHelper.Create<CurrencyMeasurementComparisonDTO>();
                prevPeriodOthersData.CurrencyId = ViewModelMapper.DISTRIBUTION_OTHERS_ID;

                var currentPeriodOthersData = othersData.ElementAt(1).First();
                currentPeriodOthersData.CurrencyId = ViewModelMapper.DISTRIBUTION_OTHERS_ID;

                prevPeriodData = prevPeriodData.Append(prevPeriodOthersData);
                currentPeriodData = currentPeriodData.Append(currentPeriodOthersData);
            }

            return (prevPeriodData, currentPeriodData);
        }
    }
}