using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Mappers;
using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;

namespace Fanex.Athena.AnalyticsAPI.Performance.Extensions
{
    public static class MetaModelExtension
    {
        public static async Task BindTopDistributionsToFilterMetaModel<TService>(this PerformanceFilterMetaModel filter, TService performanceService, int numberOfTop = 5)
            where TService : IPerformanceService, IPerformanceDistributionService
        {
            var topDistributions = await performanceService.GetTopDistributions(filter, numberOfTop);
            filter.TopDistributions = topDistributions.Select(_ => new DistributionMetaModel
            {
                DisplayId = _.topDistributionId,
                DisplayName = _.topDistributionName,
                CurrentPeriodDistributions = new List<IdentityMetaModel> { new IdentityMetaModel { Id = _.topDistributionId, Name = _.topDistributionName } },
                PreviousPeriodDistributions = new List<IdentityMetaModel> { new IdentityMetaModel { Id = _.topDistributionId, Name = _.topDistributionName } }
            }).ToList();

            var otherDistribution = filter.TopDistributions.FirstOrDefault(_ => _.DisplayId == ViewModelMapper.DISTRIBUTION_OTHERS_ID);

            if (otherDistribution != null)
            {
                var dicTopDistributions = topDistributions.ToDictionary(_ => _.topDistributionId, _ => _);

                var (prevPeriodData, currentPeriodData) = await performanceService.GetDistributionRawData(filter);
                var prevPeriodOthersData = prevPeriodData.Where(_ => !_.IsSum && !dicTopDistributions.ContainsKey(_.Id));
                var currentPeriodOthersData = currentPeriodData.Where(_ => !_.IsSum && !dicTopDistributions.ContainsKey(_.Id));

                otherDistribution.PreviousPeriodDistributions = prevPeriodOthersData.Any() ? prevPeriodOthersData.Select(_ => new IdentityMetaModel { Id = _.Id, Name = _.Name }) : null;
                otherDistribution.CurrentPeriodDistributions = currentPeriodOthersData.Select(_ => new IdentityMetaModel { Id = _.Id, Name = _.Name });
            }
        }
    }
}