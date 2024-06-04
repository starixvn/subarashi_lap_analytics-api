using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Services;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories;
using MediatR;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
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