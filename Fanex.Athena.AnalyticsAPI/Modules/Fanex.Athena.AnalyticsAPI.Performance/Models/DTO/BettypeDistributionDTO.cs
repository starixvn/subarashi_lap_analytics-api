using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class BettypeDistributionDTO : DistributionDTO
    {
        [SsasColumn("DimSportBettypeMatrix[BetTypeDisplayName]")]
        public string BettypeDisplayName { get; set; }

        [SsasColumn("DimSportBettypeMatrix[BetTypeDisplayID]")]
        public string BetTypeDisplayID { get; set; }

        [SsasColumn("DimSportBettypeMatrix[ProductCode]")]
        public string ProductCode { get; set; }

        [SsasColumn("DimSportBettypeMatrix[ProductID]")]
        public string ProductId { get; set; }

        public override string Id => BetTypeDisplayID == "0" ? BetTypeDisplayID : $"{ProductId}@{BetTypeDisplayID}";

        public override string Name => $"{BettypeDisplayName} ({ProductCode})";
    }
}