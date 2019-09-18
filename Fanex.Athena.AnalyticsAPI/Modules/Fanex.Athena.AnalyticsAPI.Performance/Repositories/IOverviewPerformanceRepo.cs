using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.LGAPI.SSAS.Bridge.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Repositories
{
    public interface IOverviewPerformanceRepo
    {
        Task<IEnumerable<ModelTest>> GetTurnover(ISsasCriteria criteria);
    }
}