using System.Threading.Tasks;

using Fanex.Athena.AnalyticsAPI.Common.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
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

        public abstract Task<TTotal> GetBreakdown(GeneralFilterParam filterParam);
    }
}