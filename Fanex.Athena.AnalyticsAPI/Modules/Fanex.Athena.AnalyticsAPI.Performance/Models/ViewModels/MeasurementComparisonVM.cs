namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    public class MeasurementComparisonVM
    {
        public string Name { get; set; }
        public DifferenceMeasurement DifferenceTunrover { get; set; }
        public DifferenceMeasurement DifferenceGrossProfit { get; set; }
        public DifferenceMeasurement DifferenceMargin { get; set; }
    }

    public class DifferenceMeasurement
    {
        public int MeasurementId { get; set; }
        public double DifferenceTotal { get; set; }
        public double DifferenceDistribution { get; set; }
    }
}