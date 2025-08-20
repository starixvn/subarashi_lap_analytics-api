namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    using System.Collections.Generic;

    public class CurrenyPerformanceFilterMetaModel : PerformanceFilterMetaModel
    {
        public IEnumerable<string> Currencies { get; set; }
    }
}