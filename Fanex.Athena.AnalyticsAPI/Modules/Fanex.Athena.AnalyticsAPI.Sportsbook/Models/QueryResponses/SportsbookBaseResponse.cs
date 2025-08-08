namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Models
{
    using System.Collections.Generic;

    public class SportsbookBaseResponse
    {
        public IEnumerable<DimDistributionResponse> ByTurnover { get; set; }

        public IEnumerable<DimDistributionResponse> ByWinloss { get; set; }

        public bool IsHideSaba { get; set; }
    }
}