using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    public class ProductPerformanceFilterMetaModel : PerformanceFilterMetaModel
    {
        public IEnumerable<string> Products { get; set; }
    }
}