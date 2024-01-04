using System.Threading;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories;
using MediatR;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
    public class ByCurrencyQueryHandler : SportsbookBaseQueryHandler, IRequestHandler<ByCurrencyRequest, ByCurrencyResponse>
    {
        public ByCurrencyQueryHandler(ISportsbookRepository sportsbookRepository, ISportsbookMapper sportsbookMapper, ITranslationService translationService)
             : base(sportsbookRepository, sportsbookMapper, translationService)
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