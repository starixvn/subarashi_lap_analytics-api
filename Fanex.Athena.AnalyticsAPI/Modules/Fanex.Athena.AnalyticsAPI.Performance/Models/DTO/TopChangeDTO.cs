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

        [SsasColumn("DimCurrency[CurrencyName]")]
        public string CurrencyName { get; set; }
    }

    public class BettypeTopChangeDTO : TopChangeDTO
    {
        [SsasColumn("DimSportBettypeMatrix[BetTypeDisplayID]")]
        public int BetTypeDisplayID { get; set; }

        [SsasColumn("DimSportBettypeMatrix[BetTypeDisplayName]")]
        public string BetTypeDisplayName { get; set; }

        [SsasColumn("DimSportBettypeMatrix[ProductID]")]
        public int ProductId { get; set; }

        [SsasColumn("DimSportBettypeMatrix[ProductName]")]
        public int ProductName { get; set; }

        public string Key { get { return $"{ProductId}_{BetTypeDisplayID}"; } }

        public string DisplayName { get { return $"{BetTypeDisplayName} ({ProductName})";  } }
    }

    public class PlatformTopChangeDTO : TopChangeDTO
    {
        [SsasColumn("FactSitePerformance[PlatformID]")]
        public int PlatformId { get; set; }

        [SsasColumn("DimPlatForm[Platform]")]
        public string PlatformName { get; set; }
    }

    public class ProductTopChangeDTO : TopChangeDTO
    {
        [SsasColumn("DimSportBettypeMatrix[ProductID]")]
        public int ProductId { get; set; }

        [SsasColumn("DimSportBettypeMatrix[ProductName]")]
        public string ProductName { get; set; }
    }
}