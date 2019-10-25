using System;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    public class CurrenyPerformanceFilterMetalModel : PerformanceFilterMetaModel
    {
        public IEnumerable<string> Currencies { get; set; }

        public IEnumerable<Tuple<string, string>> TopDistributions { get; set; }
    }
}