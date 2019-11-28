namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    public class TopChangeVM
    {
        public string PerspectiveId { get; set; }
        public string PerspectiveName { get; set; }
        public double Total { get; set; }
        public double ChangePercent { get; set; }
    }
}