namespace Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels
{
    public class GeneralFilterMetaModel
    {
        public string SiteName { get; set; }

        public MonthRangeMetaModel CurrentPeriod { get; set; }

        public MonthRangeMetaModel PreviousPeriod { get; set; }

        public string BaseCurrencyName { get; set; }
    }
}