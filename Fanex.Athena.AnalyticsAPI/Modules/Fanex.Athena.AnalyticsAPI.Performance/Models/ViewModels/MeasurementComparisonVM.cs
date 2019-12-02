using Fanex.Athena.AnalyticsAPI.Common.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    public class MeasurementComparisonVM
    {
        public string Name { get; set; }
        public DifferenceMeasurement DifferenceTurnover { get; set; }
        public DifferenceMeasurement DifferenceGrossProfit { get; set; }
        public DifferenceMeasurement DifferenceMargin { get; set; }
    }

    public class DifferenceMeasurement
    {
        public double DifferenceTotal { get; set; }
        public double DifferenceDistribution { get; set; }
    }
}