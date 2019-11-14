using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class BettypeMeasurementPeriodDTO : MeasurementPeriodDTO
    {
        [SsasColumn("DimSportBettypeMatrix[BetTypeDisplayName]")]
        public string BettypeDisplayName { get; set; }

        [SsasColumn("DimSportBettypeMatrix[BetTypeDisplayID]")]
        public string BetTypeDisplayID { get; set; }

        [SsasColumn("DimSportBettypeMatrix[ProductCode]")]
        public string ProductCode { get; set; }

        [SsasColumn("DimSportBettypeMatrix[ProductID]")]
        public string ProductId { get; set; }

        public override string Id => $"{ProductId}@{BetTypeDisplayID}";

        public override string Name => $"{BettypeDisplayName} ({ProductCode})";
    }
}