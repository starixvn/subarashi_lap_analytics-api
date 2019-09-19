namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    public class OverviewTotalResultVM
    {
        public string BaseCurrency { get; set; }
        public string Measurement { get; set; }
        public decimal? Value { get; set; }
        public double? PercentChange { get; set; }
    }
}