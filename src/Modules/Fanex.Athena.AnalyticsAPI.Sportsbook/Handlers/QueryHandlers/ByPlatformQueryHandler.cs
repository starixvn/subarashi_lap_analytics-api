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

    public class ByPlatformQueryHandler : SportsbookBaseQueryHandler, IRequestHandler<ByPlatformRequest, ByPlatformResponse>
    {
        public ByPlatformQueryHandler(
            ISportsbookRepository sportsbookRepository,
            ISportsbookMapper sportsbookMapper,
            ITranslationService translationService,
            IAuthInfoService authInfoService)
            : base(sportsbookRepository, sportsbookMapper, translationService, authInfoService)
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