using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Repositories;
using Microsoft.Extensions.Options;

namespace Fanex.Athena.AnalyticsAPI.Performance.Repositories
{
    public class PerformanceRepo : BaseRepo
    {
        public PerformanceRepo(IOptions<SSASSetting> setting)
            : base(setting) { }

        protected override string ModuleId => "Performance";
    }
}