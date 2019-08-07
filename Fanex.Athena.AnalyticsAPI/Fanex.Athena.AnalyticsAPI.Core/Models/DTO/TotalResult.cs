using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Core.Models.DTO
{
    public class TotalResult : AnalyticResult<ChartColumnResult<PartChartColumnResult>>
    {
        public IEnumerable<TotalResultItem> TableResult { get; set; }
    }

    public class TotalResultItem
    {
        public int Year { get; set; }
        public decimal Total { get; set; }
        public decimal Avg { get; set; }
    }
}