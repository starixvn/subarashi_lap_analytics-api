namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    public class SummaryVM
    {
        public int MeasurementId { get; set; }
        public double? Value { get; set; }
        public double? ChangePercent { get; set; }
    }
}