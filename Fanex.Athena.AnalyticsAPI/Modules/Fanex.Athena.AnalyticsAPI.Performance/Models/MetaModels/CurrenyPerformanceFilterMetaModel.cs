using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    public class CurrenyPerformanceFilterMetaModel : PerformanceFilterMetaModel
    {
        public IEnumerable<string> Currencies { get; set; }
    }
}