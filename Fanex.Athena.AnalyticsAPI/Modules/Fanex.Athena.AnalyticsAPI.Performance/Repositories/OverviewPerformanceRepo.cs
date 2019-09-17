using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Repositories
{
    public class OverviewPerformanceRepo : PerformanceRepo, IOverviewPerformanceRepo
    {
        public OverviewPerformanceRepo(IOptions<SSASSetting> setting)
           : base(setting) { }
    }
}