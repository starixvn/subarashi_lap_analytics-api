namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Models.Criteria
{
    using Common.Models.Criteria;
    using Constants;

    public class SportsbookCriteria : AnalyticCriteria
    {
        public byte Type { get; set; }

        public int SportType { get; set; }

        public override string GetSettingKey() => "LAP_Sportbook_Performance_MeasureDistributionBySite";

        public override bool IsValid()
        {
#pragma warning disable S1067 // Expressions should not be too complex
            return (Type >= TabType.SPORT && Type <= TabType.PLATFORM)
                && (SportType >= Constants.SportType.ALL && SportType <= Constants.SportType.BASKETBALL)
                && base.IsValid();
#pragma warning restore S1067 // Expressions should not be too complex
        }
    }
}