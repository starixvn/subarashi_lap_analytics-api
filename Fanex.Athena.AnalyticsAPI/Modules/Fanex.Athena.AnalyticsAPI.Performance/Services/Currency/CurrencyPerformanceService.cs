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

        public async Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(CurrenyPerformanceFilterMetalModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, CurrenyPerformanceFilterMetalModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToPerformanceCriteria<CurrencyDistributionCriteria, CurrenyPerformanceFilterMetalModel>(filter);

            var dtos = await _performanceRepository.QueryMultipleAsync<CurrencyDistributionDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToDistributionVM(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1));

            return result;
        }

        public async Task<IEnumerable<MeasurementComparisonVM>> GetMeasurementDifferenceWithComparison(CurrenyPerformanceFilterMetalModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyMeasurementComparisonCriteria, CurrenyPerformanceFilterMetalModel>(filter, false);
            var taskPrevPeriod = _performanceRepository.QueryAsync<CurrencyMeasurementComparisonDTO>(criteriaPrevPeriod);

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyMeasurementComparisonCriteria, CurrenyPerformanceFilterMetalModel>(filter);
            var taskCurrentPeriod = _performanceRepository.QueryAsync<CurrencyMeasurementComparisonDTO>(criteriaCurrentPeriod);

            var result = _viewModelMapper.ToMeasurementComparisonVM(await taskPrevPeriod, await taskCurrentPeriod, filter.TopDistributions);

            return result;
        }

        public async Task<IEnumerable<MeasurementPeriodComparisonGrowthVM>> GetPeriodGrowthWithComparison(CurrenyPerformanceFilterMetalModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyPeriodGrowthCriteria, CurrenyPerformanceFilterMetalModel>(filter, false);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyPeriodGrowthCriteria, CurrenyPerformanceFilterMetalModel>(filter);

            var dtos = await _performanceRepository.QueryMultipleAsync<CurrencyMeasurementPeriodDTO>(criteriaPrevPeriod, criteriaCurrentPeriod);

            var result = _viewModelMapper.ToMeasurementGrowthPeriodVM(dtos.ElementAtOrDefault(0), dtos.ElementAtOrDefault(1), filter.TopDistributions);

            return result;
        }

        public async Task<IEnumerable<MeasurementGrowthVM>> GetDetailMonthGrowthWithComparison(CurrenyPerformanceFilterMetalModel filter)
        {
            var newFilter = MetaModelHelper.BackToCurrentPeriodByMonth(filter, 1);
            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyTimeGrowthCriteria, CurrenyPerformanceFilterMetalModel>(newFilter);
            var taskCurrentPeriod = _performanceRepository.QueryAsync<MeasurementTimeDTO>(criteriaCurrentPeriod);

            var result = _viewModelMapper.ToMeasurementGrowthTimeVM(await taskCurrentPeriod,
                DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.FromMonth),
                DateTimeHelper.ParseFromFormatDateToDate(filter.CurrentPeriod.ToMonth)
                );

            return result;
        }
    }
}