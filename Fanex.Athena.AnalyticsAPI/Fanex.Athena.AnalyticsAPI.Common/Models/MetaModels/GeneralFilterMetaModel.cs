namespace Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels
{
    public class GeneralFilterMetaModel : IBaseMetaModel
    {
        public string SiteName { get; set; } = "12BET";

        public MonthRangeMetaModel CurrentPeriod { get; set; }

        public MonthRangeMetaModel PreviousPeriod { get; set; }

        public string BaseCurrencyName { get; set; }
    }
}