namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Models
{
    using MediatR;

    public class ByLeagueSoccerRequest : SportsbookBaseRequest, IRequest<ByLeagueResponse>
    {
        public override byte TabType => Constants.TabType.LEAGUE;

        public override int SportType => Constants.SportType.SOCCER;
    }
}