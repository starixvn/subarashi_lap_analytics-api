using Fanex.Athena.AnalyticsAPI.Common.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Models
{
    public class SportsbookDTO
    {
        [SsasColumn("[ID]")]
        public int Id { get; set; }

        [SsasColumn("[Name]")]
        public string Name { get; set; }

        [SsasColumn("[IsSaba]")]
        public bool IsSaba { get; set; }

        [SsasColumn("[Turnover]")]
        public double Turnover { get; set; }

        [SsasColumn("[TurnoverNoDraw]")]
        public double TurnoverNoDraw { get; set; }

        [SsasColumn("[Winloss]")]
        public double Winloss { get; set; }
    }
}