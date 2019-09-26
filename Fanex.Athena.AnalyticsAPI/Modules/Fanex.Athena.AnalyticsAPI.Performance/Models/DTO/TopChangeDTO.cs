using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class TopChangeDTO
    {
        [SsasColumn("DimSite[Licensee Name]")]
        public string LicenseeName { get; set; }

        [SsasColumn("[Measurement]")]
        public decimal Total { get; set; }
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
}