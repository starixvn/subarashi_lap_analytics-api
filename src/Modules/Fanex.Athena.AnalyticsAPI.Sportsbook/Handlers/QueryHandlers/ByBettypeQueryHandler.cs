namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Constants;
    using Common.Services;
    using Core.Services;
    using Mappers;
    using MediatR;
    using Models;
    using Repositories;

    public class ByBettypeQueryHandler : SportsbookBaseQueryHandler,
        IRequestHandler<ByBettypeSoccerRequest, ByBettypeResponse>,
        IRequestHandler<ByBettypeBasketballRequest, ByBettypeResponse>
    {
        public ByBettypeQueryHandler(
            ISportsbookRepository sportsbookRepository,
            ISportsbookMapper sportsbookMapper,
            ITranslationService translationService,
            IAuthInfoService authInfoService)
            : base(sportsbookRepository, sportsbookMapper, translationService, authInfoService)
        {
        }

        protected override string TranslationType => TranslationConst.BettypeType;

        public async Task<ByBettypeResponse> Handle(ByBettypeSoccerRequest request, CancellationToken cancellationToken)
        {
            var rawData = await GetRawData(request);
            rawData = rawData.Where(_ => _.IsSaba == request.IsSaba);
            return new ByBettypeResponse
            {
                ByTurnover = Translate(_sportsbookMapper.ToTurnoverResponse(rawData, includeOther: true)),
                ByWinloss = Translate(_sportsbookMapper.ToWinlossResponse(rawData, includeOther: true)),
                IsHideSaba = IsHideSaba
            };
        }

        public async Task<ByBettypeResponse> Handle(ByBettypeBasketballRequest request, CancellationToken cancellationToken)
        {
            var rawData = await GetRawData(request);
            rawData = rawData.Where(_ => _.IsSaba == request.IsSaba);
            return new ByBettypeResponse
            {
                ByTurnover = Translate(_sportsbookMapper.ToTurnoverResponse(rawData, includeOther: true)),
                ByWinloss = Translate(_sportsbookMapper.ToWinlossResponse(rawData, includeOther: true)),
                IsHideSaba = IsHideSaba
            };
        }
    }
}