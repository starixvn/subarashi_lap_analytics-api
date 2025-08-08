namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Services;
    using Core.Services;
    using Mappers;
    using MediatR;
    using Models;
    using Repositories;

    public class ByCurrencyQueryHandler : SportsbookBaseQueryHandler, IRequestHandler<ByCurrencyRequest, ByCurrencyResponse>
    {
        public ByCurrencyQueryHandler(
            ISportsbookRepository sportsbookRepository,
            ISportsbookMapper sportsbookMapper,
            ITranslationService translationService,
            IAuthInfoService authInfoService)
             : base(sportsbookRepository, sportsbookMapper, translationService, authInfoService)
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