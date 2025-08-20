namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Models
{
    using MediatR;

    public class BySportRequest : SportsbookBaseRequest, IRequest<BySportResponse>
    {
        public override byte TabType => Constants.TabType.SPORT;

        public override int SportType => Constants.SportType.ALL;
    }
}