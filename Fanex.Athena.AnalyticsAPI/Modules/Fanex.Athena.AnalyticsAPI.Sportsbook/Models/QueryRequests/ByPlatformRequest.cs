using MediatR;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Models
{
    public class ByPlatformRequest : SportsbookBaseRequest, IRequest<ByPlatformResponse>
    {
        public override byte TabType => Constants.TabType.PLATFORM;
        public override int SportType => Constants.SportType.ALL;
    }
}