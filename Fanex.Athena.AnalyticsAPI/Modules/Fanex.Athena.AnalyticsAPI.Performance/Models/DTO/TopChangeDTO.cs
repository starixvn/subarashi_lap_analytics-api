namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    using Common.Attributes;

    public abstract class TopChangeDTO : IIdentifiable, IMeasurement
    {
        [SsasColumn("[Measurement]")]
        public double Total { get; set; }

        public abstract string Id { get; }

        public abstract string Name { get; }
    }

    public class CurrencyTopChangeDTO : TopChangeDTO
    {
        [SsasColumn("DimCurrency[CurrencyID]")]
        public int CurrencyId { get; set; }

        [SsasColumn("DimCurrency[CurrencyName]")]
        public string CurrencyName { get; set; }

        public override string Id => CurrencyId.ToString();

        public override string Name => CurrencyName;
    }

    public class BettypeTopChangeDTO : TopChangeDTO
    {
        [SsasColumn("DimSportBettypeMatrix[BetTypeDisplayID]")]
        public int BetTypeDisplayID { get; set; }

        [SsasColumn("DimSportBettypeMatrix[BetTypeDisplayName]")]
        public string BetTypeDisplayName { get; set; }

        [SsasColumn("DimSportBettypeMatrix[ProductID]")]
        public int ProductId { get; set; }

        [SsasColumn("DimSportBettypeMatrix[ProductCode]")]
        public string ProductCode { get; set; }

        public override string Id => $"{ProductId}@{BetTypeDisplayID}";

        public override string Name => $"{BetTypeDisplayName} ({ProductCode})";
    }

    public class PlatformTopChangeDTO : TopChangeDTO
    {
        [SsasColumn("FactSitePerformance[PlatformID]")]
        public int PlatformId { get; set; }

        [SsasColumn("DimPlatForm[Platform]")]
        public string PlatformName { get; set; }

        public override string Id => PlatformId.ToString();

        public override string Name => PlatformName;
    }

    public class ProductTopChangeDTO : TopChangeDTO
    {
        [SsasColumn("DimSportBettypeMatrix[ProductID]")]
        public int ProductId { get; set; }

        [SsasColumn("DimSportBettypeMatrix[ProductName]")]
        public string ProductName { get; set; }

        public override string Id => ProductId.ToString();

        public override string Name => ProductName;
    }
}