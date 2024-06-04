using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Helpers;
using Fanex.Athena.AnalyticsAPI.Core.Services;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Handlers
{
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