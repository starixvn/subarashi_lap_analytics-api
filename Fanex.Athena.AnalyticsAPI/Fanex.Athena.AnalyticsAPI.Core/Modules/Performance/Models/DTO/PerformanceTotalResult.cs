using Fanex.Athena.AnalyticsAPI.Core.Models.DTO;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Models.DTO
{
    public class PerformanceTotalResult : TotalResult
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