namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Models
{
    using Core.Models.MetaModels;
    using Newtonsoft.Json;

    public abstract class SportsbookBaseRequest : ISiteNameMetaModel
    {
        [JsonIgnore]
        public string SiteName { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string BaseCurrency { get; set; }

        public abstract byte TabType { get; }

        public abstract int SportType { get; }
    }
}