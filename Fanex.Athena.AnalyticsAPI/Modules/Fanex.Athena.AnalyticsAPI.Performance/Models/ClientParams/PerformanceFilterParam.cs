using Fanex.Athena.AnalyticsAPI.Core.Models.ClientParams;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ClientParams
{
    public class PerformanceFilterParam : GeneralFilterParam
    {
        public SelectParam Measurement { get; set; }
    }
}