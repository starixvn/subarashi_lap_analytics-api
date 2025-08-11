namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    using Common.Attributes;

    public class ProductDistributionDTO : DistributionDTO
    {
        [SsasColumn("DimSportBettypeMatrix[ProductID]")]
        public string ProductId { get; set; }

        [SsasColumn("DimSportBettypeMatrix[ProductName]")]
        public string ProductName { get; set; }

        public override string Id => ProductId;

        public override string Name => ProductName;
    }
}