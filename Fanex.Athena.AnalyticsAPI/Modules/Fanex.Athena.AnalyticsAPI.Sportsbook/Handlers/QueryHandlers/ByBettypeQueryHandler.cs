using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
    public class ByBettypeQueryHandler : SportsbookBaseQueryHandler,
        IRequestHandler<ByBettypeSoccerRequest, ByBettypeResponse>,
        IRequestHandler<ByBettypeBasketballRequest, ByBettypeResponse>
    {
        public ByBettypeQueryHandler(ISportsbookRepository sportsbookRepository, ISportsbookMapper sportsbookMapper)
            : base(sportsbookRepository, sportsbookMapper)
        {
        }

        public async Task<ByBettypeResponse> Handle(ByBettypeSoccerRequest request, CancellationToken cancellationToken)
        {
            var rawData = await GetRawData(request);
            rawData = rawData.Where(_ => _.IsSaba == request.IsSaba);
            return new ByBettypeResponse
            {
                ByTurnover = _sportsbookMapper.ToTurnoverResponse(rawData, includeOther: true),
                ByWinloss = _sportsbookMapper.ToWinlossResponse(rawData, includeOther: true)
            };
        }

        public async Task<ByBettypeResponse> Handle(ByBettypeBasketballRequest request, CancellationToken cancellationToken)
        {
            var rawData = await GetRawData(request);
            rawData = rawData.Where(_ => _.IsSaba == request.IsSaba);
            return new ByBettypeResponse
            {
                ByTurnover = _sportsbookMapper.ToTurnoverResponse(rawData, includeOther: true),
                ByWinloss = _sportsbookMapper.ToWinlossResponse(rawData, includeOther: true)
            };
        }
    }
}