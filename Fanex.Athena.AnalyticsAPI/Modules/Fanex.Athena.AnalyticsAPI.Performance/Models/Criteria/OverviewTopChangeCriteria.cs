using Fanex.Athena.AnalyticsAPI.Common.Models.Criteria;
using Fanex.Data;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    public class OverviewTopChangeCriteria : AnalyticCriteria
    {
        private static string[] spNames = new [] 
        {
            "LAP_Performance_OverviewCurencyBySite",
            "LAP_Performance_OverviewBetypeBySite",
            "LAP_Performance_OverviewPlatFormBySite",
        };

        public int MeasurementId { get; set; }

        [SpParam(Ignored = true)]
        public int Perspective { get; set; }

        public override string GetSettingKey()
        {
            return spNames[Perspective];
        }
    }
}