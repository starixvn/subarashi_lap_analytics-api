using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
    public class ByCurrencyQueryHandler : SportsbookBaseQueryHandler, IRequestHandler<ByCurrencyRequest, ByCurrencyResponse>
    {
        public ByCurrencyQueryHandler(ISportsbookRepository sportsbookRepository, ISportsbookMapper sportsbookMapper)
            : base(sportsbookRepository, sportsbookMapper)
        {
        }

        public async Task<ByCurrencyResponse> Handle(ByCurrencyRequest request, CancellationToken cancellationToken)
        {
            var rawData = await GetRawData(request);
            return new ByCurrencyResponse
            {
                ByTurnover = _sportsbookMapper.ToTurnoverResponse(rawData, includeOther: true),
                ByWinloss = _sportsbookMapper.ToWinlossResponse(rawData, includeOther: true)
            };
        }
    }
}