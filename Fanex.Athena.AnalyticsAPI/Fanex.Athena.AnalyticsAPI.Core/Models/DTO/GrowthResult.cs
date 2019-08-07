using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Core.Models.DTO
{
    public class GrowthResult : AnalyticResult<ChartColumnResult<PartChartColumnResult>>
    {
        public IEnumerable<GrowthResultItem> TableResult { get; set; }
    }

    public class GrowthResultItem
    {
        public int Year { get; set; }
        public double Avg { get; set; }
    }
}