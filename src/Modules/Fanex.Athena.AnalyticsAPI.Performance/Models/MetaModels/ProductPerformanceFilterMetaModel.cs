namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    using System.Collections.Generic;

    public class ProductPerformanceFilterMetaModel : PerformanceFilterMetaModel
    {
        public IEnumerable<string> Products { get; set; }
    }
}