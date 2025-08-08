namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Constants;
    using Common.Services;
    using Constants;
    using Core.Services;
    using Mappers;
    using MediatR;
    using Models;
    using Repositories;

    public class ByLeagueQueryHandler : SportsbookBaseQueryHandler,
        IRequestHandler<ByLeagueSoccerRequest, ByLeagueResponse>,
        IRequestHandler<ByLeagueBasketballRequest, ByLeagueResponse>,
        IRequestHandler<ByLeagueCricketRequest, ByLeagueResponse>
    {
        public ByLeagueQueryHandler(
            ISportsbookRepository sportsbookRepository,
            ISportsbookMapper sportsbookMapper,
            ITranslationService translationService,
            IAuthInfoService authInfoService)
            : base(sportsbookRepository, sportsbookMapper, translationService, authInfoService)
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