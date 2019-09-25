namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    public class TopChangeVM
    {
        public string PerspectiveName { get; set; }
        public decimal? Total { get; set; }
        public double? PercentChange { get; set; }
    }
}