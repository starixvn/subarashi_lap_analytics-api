using Fanex.Athena.AnalyticsAPI.Common.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class ProductMeasurementPeriodDTO : MeasurementPeriodDTO
    {
        [SsasColumn("DimSportBettypeMatrix[ProductID]")]
        public string ProductId { get; set; }

        [SsasColumn("DimSportBettypeMatrix[ProductName]")]
        public string ProductName { get; set; }

        public override string Id => ProductId;

        public override string Name => ProductName;
    }
}