using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories;
using MediatR;
using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
    public class ByLeagueQueryHandler : SportsbookBaseQueryHandler,
        IRequestHandler<ByLeagueSoccerRequest, ByLeagueResponse>,
        IRequestHandler<ByLeagueBasketballRequest, ByLeagueResponse>
    {
        public ByLeagueQueryHandler(ISportsbookRepository sportsbookRepository, ISportsbookMapper sportsbookMapper)
            : base(sportsbookRepository, sportsbookMapper)
        {
        }

        public async Task<ByLeagueResponse> Handle(ByLeagueSoccerRequest request, CancellationToken cancellationToken)
        {
            var rawData = await GetRawData(request);
            return new ByLeagueResponse
            {
                ByTurnover = _sportsbookMapper.ToTurnoverResponse(rawData, includeOther: false),
                ByWinloss = _sportsbookMapper.ToWinlossResponse(rawData, includeOther: false)
            };
        }

        public async Task<ByLeagueResponse> Handle(ByLeagueBasketballRequest request, CancellationToken cancellationToken)
        {
            var rawData = await GetRawData(request);
            return new ByLeagueResponse
            {
                ByTurnover = _sportsbookMapper.ToTurnoverResponse(rawData, includeOther: false),
                ByWinloss = _sportsbookMapper.ToWinlossResponse(rawData, includeOther: false)
            };
        }
    }
}