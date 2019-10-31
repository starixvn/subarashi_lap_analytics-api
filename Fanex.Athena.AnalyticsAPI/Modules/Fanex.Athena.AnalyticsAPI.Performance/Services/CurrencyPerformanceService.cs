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
            ICurrencyPerformanceVMMapper viewModelMapper
            ) : base(context, performanceRepository)
        {
            _viewModelMapper = viewModelMapper;
        }

        public async Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(CurrenyPerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, CurrenyPerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, CurrenyPerformanceFilterMetaModel>(filter);

            var dtos = await _performanceRepository.QueryMultipleAsync<CurrencyDistributionDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToDistributionVM(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1), MapToTopDistributions(filter.TopDistributions));

            return result;
        }

        public async Task<IEnumerable<MeasurementComparisonVM>> GetMeasurementDifferenceWithComparison(CurrenyPerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyMeasurementComparisonCriteria, CurrenyPerformanceFilterMetaModel>(filter, false);
            var taskPrevPeriod = _performanceRepository.QueryAsync<CurrencyMeasurementComparisonDTO>(criteriaPrevPeriod);

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyMeasurementComparisonCriteria, CurrenyPerformanceFilterMetaModel>(filter);
            var taskCurrentPeriod = _performanceRepository.QueryAsync<CurrencyMeasurementComparisonDTO>(criteriaCurrentPeriod);

            var result = _viewModelMapper.ToMeasurementComparisonVM(await taskPrevPeriod, await taskCurrentPeriod, MapToTopDistributions(filter.TopDistributions));

            return result;
        }

        public async Task<IEnumerable<MeasurementPeriodComparisonGrowthVM>> GetPeriodGrowthWithComparison(CurrenyPerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyPeriodGrowthCriteria, CurrenyPerformanceFilterMetaModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyPeriodGrowthCriteria, CurrenyPerformanceFilterMetaModel>(filter);

            var dtos = await _performanceRepository.QueryMultipleAsync<CurrencyMeasurementPeriodDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToMeasurementGrowthPeriodVM(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1), MapToTopDistributions(filter.TopDistributions));

            return result;
        }

        public async Task<IEnumerable<MeasurementGrowthVM>> GetDetailMonthGrowthWithComparison(CurrenyPerformanceFilterMetaModel filter)
        {
            var newFilter = MetaModelHelper.BackToPeriodByMonth(filter, 1);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyTimeGrowthCriteria, CurrenyPerformanceFilterMetaModel>(newFilter);
            var taskCurrentPeriod = _performanceRepository.QueryAsync<MeasurementTimeDTO>(criteriaCurrentPeriod);

            var result = _viewModelMapper.ToMeasurementGrowthTimeVM(await taskCurrentPeriod,
                DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.FromMonth),
                DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.ToMonth)
                );

            return result;
        }

        public async Task<IDictionary<string, object>> GetAllDetailMonthGrowthWithComparison(CurrenyPerformanceFilterMetaModel filter)
        {
            var result = new Dictionary<string, object>(filter.TopDistributions.Count());

            var cloneFilter = MetaModelHelper.Clone(filter);
            cloneFilter = MetaModelHelper.BackToPeriodByMonth(cloneFilter, 1);
            cloneFilter = MetaModelHelper.BackToPeriodByMonth(cloneFilter, 1, false);

            foreach (var topDistribution in filter.TopDistributions)
            {
                cloneFilter.Currencies = topDistribution.PreviousPeriodDistributions.Select(_ => _.Name);
                var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyTimeGrowthCriteria, CurrenyPerformanceFilterMetaModel>(cloneFilter, false);

                cloneFilter.Currencies = topDistribution.CurrentPeriodDistributions.Select(_ => _.Name);
                var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyTimeGrowthCriteria, CurrenyPerformanceFilterMetaModel>(cloneFilter);

                var dtos = await _performanceRepository.QueryMultipleAsync<MeasurementTimeDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

                var prevPeriodResult = _viewModelMapper.ToMeasurementGrowthTimeVM(dtos.ElementAtOrDefault(0),
                        DateTimeHelper.ParseFromFormatDateToDate(filter.PreviousPeriod.FromMonth),
                        DateTimeHelper.ParseFromFormatDateToDate(filter.PreviousPeriod.ToMonth)
                        );

                var currentPeriodResult = _viewModelMapper.ToMeasurementGrowthTimeVM(dtos.ElementAtOrDefault(1),
                       DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.FromMonth),
                       DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.ToMonth)
                       );

                result.Add(GetFieldNameOfMonthGrowthObject(topDistribution.DisplayName, filter.PreviousPeriod), prevPeriodResult);
                result.Add(GetFieldNameOfMonthGrowthObject(topDistribution.DisplayName, filter.CurrentPeriod), currentPeriodResult);
            }

            return result;
        }

        public async Task<IEnumerable<OverallMeasurementVM>> GetOverallMeasurement(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, PerformanceFilterMetaModel>(filter, false);
            criteriaPrevPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, PerformanceFilterMetaModel>(filter);
            criteriaCurrentPeriod.DrillDown = DrillDownConst.NONE_DRILL_DOWN;

            var dtos = await _performanceRepository.QueryMultipleAsync<TotalDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToOverallMeasurement(dtos.ElementAtOrDefault(0)?.FirstOrDefault(), dtos.ElementAtOrDefault(1)?.FirstOrDefault());

            return result;
        }

        public async Task<IEnumerable<(string topDistributionId, string topDistributionName)>> GetTopDistributions(PerformanceFilterMetaModel filter)
        {
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, PerformanceFilterMetaModel>(filter);
            var dtos = await _performanceRepository.QueryAsync<CurrencyDistributionDTO>(criteriaCurrentPeriod);

            var result = GetTopDistributions(dtos);

            return result;
        }
    }
}