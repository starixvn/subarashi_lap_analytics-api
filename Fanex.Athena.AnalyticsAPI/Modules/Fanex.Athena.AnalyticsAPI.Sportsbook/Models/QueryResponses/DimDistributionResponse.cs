namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Models
{
    public class DimDistributionResponse
    {
        public int Id { get; set; }
        public bool IsSaba { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double Distribution { get; set; }
        public double Margin { get; set; }
    }
}