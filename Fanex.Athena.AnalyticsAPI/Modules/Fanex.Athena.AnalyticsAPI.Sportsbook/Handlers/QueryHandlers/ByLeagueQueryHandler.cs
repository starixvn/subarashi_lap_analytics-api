using System.Threading;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Constants;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories;
using MediatR;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
    public class ByLeagueQueryHandler : SportsbookBaseQueryHandler,
        IRequestHandler<ByLeagueSoccerRequest, ByLeagueResponse>,
        IRequestHandler<ByLeagueBasketballRequest, ByLeagueResponse>,
        IRequestHandler<ByLeagueCricketRequest, ByLeagueResponse>
    {
        public ByLeagueQueryHandler(ISportsbookRepository sportsbookRepository, ISportsbookMapper sportsbookMapper, ITranslationService translationService)
            : base(sportsbookRepository, sportsbookMapper, translationService)
        {
        }

        protected override string TranslationType => TranslationConst.LeagueType;

        public async Task<ByLeagueResponse> Handle(ByLeagueSoccerRequest request, CancellationToken cancellationToken)
        {
            var rawData = await GetRawData(request);
            return new ByLeagueResponse
            {
                ByTurnover = Translate(_sportsbookMapper.ToTurnoverResponse(rawData, onlySupportedData: DomainData.SupportedSoccerLeagues, includeOther: true)),
                ByWinloss = Translate(_sportsbookMapper.ToWinlossResponse(rawData, onlySupportedData: DomainData.SupportedSoccerLeagues, includeOther: true))
            };
        }

        public async Task<ByLeagueResponse> Handle(ByLeagueBasketballRequest request, CancellationToken cancellationToken)
        {
            var rawData = await GetRawData(request);
            return new ByLeagueResponse
            {
                ByTurnover = Translate(_sportsbookMapper.ToTurnoverResponse(rawData, includeOther: false)),
                ByWinloss = Translate(_sportsbookMapper.ToWinlossResponse(rawData, includeOther: false))
            };
        }

        public async Task<ByLeagueResponse> Handle(ByLeagueCricketRequest request, CancellationToken cancellationToken)
        {
            var rawData = await GetRawData(request);
            return new ByLeagueResponse
            {
                ByTurnover = Translate(_sportsbookMapper.ToTurnoverResponse(rawData, onlySupportedData: DomainData.SupportedCricketLeagues, includeOther: true)),
                ByWinloss = Translate(_sportsbookMapper.ToWinlossResponse(rawData, onlySupportedData: DomainData.SupportedCricketLeagues, includeOther: true))
            };
        }
    }
}