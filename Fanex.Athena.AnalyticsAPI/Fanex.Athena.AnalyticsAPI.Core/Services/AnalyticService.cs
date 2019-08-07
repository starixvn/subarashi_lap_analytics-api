using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Models.ClientParams;
using Fanex.Athena.AnalyticsAPI.Core.Models.DTO;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    public abstract class AnalyticService<TTotal, TGrowth, TDistribution> : BaseService, IAnalyticService<TTotal, TGrowth, TDistribution>
        where TTotal : TotalResult
        where TGrowth : GrowthResult
        where TDistribution : DistributionResult
    {
        protected readonly IContextService _context;

        protected AnalyticService(IContextService context)
        {
            _context = context;
        }

        public abstract Task<TDistribution> GetDistribution(GeneralFilterParam filterParam);

        public abstract Task<TGrowth> GetGrowth(GeneralFilterParam filterParam);

        public abstract Task<TTotal> GetTotal(GeneralFilterParam filterParam);
    }
}