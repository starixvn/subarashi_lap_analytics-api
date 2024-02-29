using MediatR;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Models
{
    public class ByBettypeBasketballRequest : SportsbookBaseRequest, IRequest<ByBettypeResponse>
    {
        public override byte TabType => Constants.TabType.BETTYPE;

        public override int SportType => Constants.SportType.BASKETBALL;

        public bool IsSaba { get; set; }
    }
}