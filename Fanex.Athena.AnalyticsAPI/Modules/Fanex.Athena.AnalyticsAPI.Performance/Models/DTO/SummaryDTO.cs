using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class SummaryDTO
    {
        private const int Percent = 100;

        [SsasColumn("[Turnover]")]
        public double Turnover { get; set; }

        [SsasColumn("[Gross Profit]")]
        public double GrossProfit { get; set; }

        private double _margin;

        [SsasColumn("[Margin]")]
        public double Margin
        {
            get
            {
                return _margin;
            }
            set
            {
                _margin = value * Percent;
            }
        }
    }
}