using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
    public class ByPlatformQueryHandler : SportsbookBaseQueryHandler, IRequestHandler<ByPlatformRequest, ByPlatformResponse>
    {
        public ByPlatformQueryHandler(ISportsbookRepository sportsbookRepository, ISportsbookMapper sportsbookMapper)
            : base(sportsbookRepository, sportsbookMapper)
        {
        }

        public async Task<ByPlatformResponse> Handle(ByPlatformRequest request, CancellationToken cancellationToken)
        {
            var rawData = await GetRawData(request);
            return new ByPlatformResponse
            {
                ByTurnover = _sportsbookMapper.ToTurnoverResponse(rawData, includeOther: true),
                ByWinloss = _sportsbookMapper.ToWinlossResponse(rawData, includeOther: true)
            };
        }
    }
}