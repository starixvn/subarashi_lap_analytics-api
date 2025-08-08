namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Models
{
    using MediatR;

    public class ByBettypeSoccerRequest : SportsbookBaseRequest, IRequest<ByBettypeResponse>
    {
        public override byte TabType => Constants.TabType.BETTYPE;

        public override int SportType => Constants.SportType.SOCCER;

        public bool IsSaba { get; set; }
    }
}