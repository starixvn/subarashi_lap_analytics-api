using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using System.Collections.Generic;
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

        public async Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(PerformanceFilterMetaModel filter)
        {
            var criteriaPrevPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<CurrencyDistributionCriteria, PerformanceFilterMetaModel>(filter, false);
            var taskPrevPeriod = _performanceRepository.QueryAsync<CurrencyDistributionDTO>(criteriaPrevPeriod);

            var criteriaCurrentPeriod = PerformanceHelper.CriteriaHelper.ToCriteria<CurrencyDistributionCriteria, PerformanceFilterMetaModel>(filter);
            var taskCurrentPeriod = _performanceRepository.QueryAsync<CurrencyDistributionDTO>(criteriaCurrentPeriod);

            var result = _viewModelMapper.ToViewModel<DistributionDTO>(await taskPrevPeriod, await taskCurrentPeriod);

            return result;
        }
    }
}