using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Constants;
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
        public ByPlatformQueryHandler(ISportsbookRepository sportsbookRepository, ISportsbookMapper sportsbookMapper, ITranslationService translationService)
            : base(sportsbookRepository, sportsbookMapper, translationService)
        {
        }
        protected override string TranslationType => TranslationConst.PlatformType;

        public async Task<ByPlatformResponse> Handle(ByPlatformRequest request, CancellationToken cancellationToken)
        {
            var rawData = await GetRawData(request);
            return new ByPlatformResponse
            {
                ByTurnover = Translate(_sportsbookMapper.ToTurnoverResponse(rawData, excludedFromTopData: DomainData.ExcludedPlatformsFromTop, includeOther: true)),
                ByWinloss = Translate(_sportsbookMapper.ToWinlossResponse(rawData, excludedFromTopData: DomainData.ExcludedPlatformsFromTop, includeOther: true))
            };
        }
    }
}