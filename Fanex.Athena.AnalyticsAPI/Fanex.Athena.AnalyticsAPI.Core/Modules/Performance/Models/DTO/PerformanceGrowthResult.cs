using Fanex.Athena.AnalyticsAPI.Core.Models.DTO;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Models.DTO
{
    public class PerformanceGrowthResult : GrowthResult
    {
        public IEnumerable<GrowthResultItem> TableResult { get; set; }
    }

    public class GrowthResultItem
    {
        public int Year { get; set; }
        public double Avg { get; set; }
    }
}