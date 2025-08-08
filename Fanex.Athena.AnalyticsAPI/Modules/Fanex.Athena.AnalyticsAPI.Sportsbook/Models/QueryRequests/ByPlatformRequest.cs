namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Models
{
    using MediatR;

    public class ByPlatformRequest : SportsbookBaseRequest, IRequest<ByPlatformResponse>
    {
        public override byte TabType => Constants.TabType.PLATFORM;

        public override int SportType => Constants.SportType.ALL;
    }
}