using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Models
{
    public class SportsbookBaseResponse
    {
        public IEnumerable<DimDistributionResponse> ByTurnover { get; set; }

        public IEnumerable<DimDistributionResponse> ByWinloss { get; set; }
    }
}