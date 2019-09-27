using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class TopChangeDTO
    {
        [SsasColumn("[Measurement]")]
        public double Total { get; set; }
    }

    public class CurrencyTopChangeDTO : TopChangeDTO
    {
        [SsasColumn("DimCurrency[CurrencyID]")]
        public int CurrencyId { get; set; }
    }

    public class BettypeTopChangeDTO : TopChangeDTO
    {
        [SsasColumn("DimSportBettypeMatrix[BetTypeDisplayID]")]
        public int BetTypeDisplayID { get; set; }

        [SsasColumn("DimSportBettypeMatrix[BetTypeDisplayName]")]
        public string BetTypeDisplayName { get; set; }
    }

    public class PlatformTopChangeDTO : TopChangeDTO
    {
        [SsasColumn("FactSitePerformance[PlatformID]")]
        public int PlatformId { get; set; }
    }

    public class ProductTopChangeDTO : TopChangeDTO
    {
        [SsasColumn("DimSportBettypeMatrix[ProductID]")]
        public int ProductId { get; set; }
    }
}