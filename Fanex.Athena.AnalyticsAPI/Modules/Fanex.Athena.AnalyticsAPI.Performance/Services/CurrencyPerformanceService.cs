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

            var result = _viewModelMapper.ToDistributionVM(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1), MapToTopDistributions(filter.TopDistributions));

            return result;
        }

        public async Task<IEnumerable<MeasurementComparisonVM>> GetMeasurementDifferenceWithComparison(CurrenyPerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyMeasurementComparisonCriteria, CurrenyPerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyMeasurementComparisonCriteria, CurrenyPerformanceFilterMetaModel>(filter);

            var data = await _performanceRepository.QueryMultipleAsync<CurrencyMeasurementComparisonDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToMeasurementComparisonVM(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1), MapToTopDistributions(filter.TopDistributions));

            return result;
        }

        public async Task<IEnumerable<MeasurementPeriodComparisonGrowthVM>> GetPeriodGrowthWithComparison(CurrenyPerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyGrowthCriteria, CurrenyPerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyGrowthCriteria, CurrenyPerformanceFilterMetaModel>(filter);

            var data = await _performanceRepository.QueryMultipleAsync<CurrencyMeasurementPeriodDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);
            HandleRawData(data, filter);

            var result = _viewModelMapper.ToMeasurementGrowthPeriodVM(data.ElementAtOrDefault(0), data.ElementAtOrDefault(1), MapToTopDistributions(filter.TopDistributions));

            return result;
        }

        public async Task<IDictionary<string, object>> GetAllDetailMonthGrowthWithComparison(CurrenyPerformanceFilterMetaModel filter)
        {
            var result = new Dictionary<string, object>(filter.TopDistributions.Count() * 2);

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
                cloneFilter.Currencies = topDistribution.PreviousPeriodDistributions.Select(_ => _.Name);
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

        protected override async Task<IEnumerable<DistributionDTO>> GetDistributionData(PerformanceFilterMetaModel filter)
        {
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, PerformanceFilterMetaModel>(filter);
            var distributionData = await _performanceRepository.QueryAsync<CurrencyDistributionDTO>(criteriaCurrentPeriod);
            return distributionData;
        }
    }
}