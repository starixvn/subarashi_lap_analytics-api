namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Constants;
    using Common.Helpers;
    using Common.Services;
    using Core.Helpers;
    using Core.Services;
    using Mappers;
    using Models;
    using Models.Criteria;
    using Repositories;

    public class SportsbookBaseQueryHandler
    {
        protected readonly ISportsbookRepository _sportsbookRepository;
        protected readonly ISportsbookMapper _sportsbookMapper;
        protected readonly ITranslationService _translationService;
        protected readonly IAuthInfoService _authInfoService;

        protected SportsbookBaseQueryHandler(
            ISportsbookRepository sportsbookRepository,
            ISportsbookMapper sportsbookMapper,
            ITranslationService translationService,
            IAuthInfoService authInfoService)
        {
            _sportsbookRepository = sportsbookRepository;
            _sportsbookMapper = sportsbookMapper;
            _translationService = translationService;
            _authInfoService = authInfoService;
        }

        protected virtual string TranslationType => TranslationConst.NormalType;

        protected bool IsHideSaba => SabaHelper.IsHideSaba(_authInfoService.SpecialPlatform);

        protected async Task<IEnumerable<SportsbookDTO>> GetRawData(SportsbookBaseRequest request)
        {
            var criteria = GetCriteria(request);
            var rawData = await _sportsbookRepository.QueryAsync<SportsbookDTO>(criteria);
            return rawData;
        }

        protected IEnumerable<DimDistributionResponse> Translate(IEnumerable<DimDistributionResponse> responses)
        {
            _translationService.Translate(responses, IsHideSaba, TranslationType);
            return responses;
        }

        private SportsbookCriteria GetCriteria(SportsbookBaseRequest request)
        {
            return new SportsbookCriteria
            {
                SiteName = request.SiteName,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                BaseCurrencyName = CurrencyHelper.GetBasedCurrency(request.BaseCurrency),
                Type = request.TabType,
                SportType = request.SportType
            };
        }
    }
}