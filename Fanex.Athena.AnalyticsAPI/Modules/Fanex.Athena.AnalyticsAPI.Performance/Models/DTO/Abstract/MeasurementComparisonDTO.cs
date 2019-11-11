using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public abstract class MeasurementComparisonDTO : IIdentifiable
    {
        [SsasColumn("[Turnover]")]
        public double Turnover { get; set; }

        [SsasColumn("[MarginTO]")]
        public double TurnoverDistribution { get; set; }

        [SsasColumn("[GrossProfit]")]
        public double GrossProfit { get; set; }

        [SsasColumn("[MarginWL]")]
        public double GrossProfitDistribution { get; set; }

        [SsasColumn("[Margin]")]
        public double Margin { get; set; }

        public abstract string Id { get; }
        public abstract string Name { get; }
    }
}