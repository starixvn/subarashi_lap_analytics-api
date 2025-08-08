namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Models
{
    using Common.Attributes;
    using Common.Models.DataDomain;

    public class DimDistributionResponse : ISabaAble
    {
        public int Id { get; set; }

        [Translation(nameof(Id))]
        public string Name { get; set; }

        public bool IsSaba { get; set; }

        public double Value { get; set; }

        public double Distribution { get; set; }

        public double Margin { get; set; }
    }
}