namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    public class MeasurementGrowthVM
    {
        public string Time { get; set; }
        public decimal Total { get; set; }
        public double? GrowthPercent { get; set; }
    }
}