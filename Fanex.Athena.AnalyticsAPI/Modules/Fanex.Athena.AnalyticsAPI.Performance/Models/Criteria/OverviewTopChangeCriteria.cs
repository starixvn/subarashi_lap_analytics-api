using Fanex.Athena.AnalyticsAPI.Common.Models.Criteria;
using Fanex.Data;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria
{
    public class OverviewTopChangeCriteria : AnalyticCriteria, IMeasurementCriteria
    {
        private static string[] settingKeys = new [] 
        {
            "LAP_Performance_OverviewCurencyBySite",
            "LAP_Performance_OverviewBetypeBySite",
            "LAP_Performance_OverviewPlatFormBySite",
            "LAP_Performance_OverviewProductBySite"
        };

        public int MeasurementId { get; set; }

        [SpParam(Ignored = true)]
        public byte Dimension { get; set; }

        public override string GetSettingKey()
        {
            return settingKeys[Dimension];
        }
    }
}