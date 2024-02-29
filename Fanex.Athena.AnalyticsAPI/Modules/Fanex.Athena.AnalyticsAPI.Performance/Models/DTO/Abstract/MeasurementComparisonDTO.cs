using Fanex.Athena.AnalyticsAPI.Common.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public abstract class MeasurementComparisonDTO : IIdentifiable
    {
        private const int Percent = 100;

        [SsasColumn("[Turnover]")]
        public double Turnover { get; set; }

        [SsasColumn("[MarginTO]")]
        public double TurnoverDistribution { get; set; }

        [SsasColumn("[GrossProfit]")]
        public double GrossProfit { get; set; }

        [SsasColumn("[MarginWL]")]
        public double GrossProfitDistribution { get; set; }

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

        public abstract string Id { get; }

        public abstract string Name { get; }
    }
}