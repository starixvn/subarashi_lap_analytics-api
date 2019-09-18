using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Repositories;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.LGAPI.SSAS.Bridge.Utils;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Repositories
{
    public class PerformanceRepo : BaseRepo
    {
        public PerformanceRepo(IOptions<SSASSetting> setting)
            : base(setting) { }

        protected override string ModuleId => "Performance";

        public Task<IEnumerable<ModelTest>> GetTurnover(ISsasCriteria criteria)
        {
            return QueryAsync<ModelTest>(criteria);
        }
    }
}