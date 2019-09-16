using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Microsoft.Extensions.Options;

namespace Fanex.Athena.AnalyticsAPI.Performance.Repositories
{
    public class OverviewPerformanceRepo : PerformanceRepo, IOverviewPerformanceRepo
    {
        public OverviewPerformanceRepo(IOptions<SSASSetting> setting)
           : base(setting) { }


    }
}