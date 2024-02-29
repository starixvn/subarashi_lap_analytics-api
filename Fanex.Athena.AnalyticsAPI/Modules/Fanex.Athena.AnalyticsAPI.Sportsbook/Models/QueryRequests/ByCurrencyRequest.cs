using MediatR;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Models
{
    public class ByCurrencyRequest : SportsbookBaseRequest, IRequest<ByCurrencyResponse>
    {
        public override byte TabType => Constants.TabType.CURRENCY;

        public override int SportType => Constants.SportType.ALL;
    }
}