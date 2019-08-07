using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Core.Models.ClientParams
{
    public class GeneralFilterParam : PeriodFilterParam
    {
        public int BaseCurrency { get; set; }
        public IEnumerable<int> Platforms { get; set; }
        public IEnumerable<int> BetTypes { get; set; }
        public IEnumerable<int> Products { get; set; }
        public IEnumerable<int> Currencies { get; set; }
    }
}