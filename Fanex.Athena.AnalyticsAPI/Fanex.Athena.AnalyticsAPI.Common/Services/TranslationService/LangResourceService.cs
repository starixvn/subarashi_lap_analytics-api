using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Resources.Lang;
using Fanex.RemotingRef;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class LangResourceService : ILangResourceService
    {
        private static readonly string[] EXCEPTION_BETTYPE_IDs = new[] { "0" };
        private static readonly string[] EXCEPTION_SPORT_IDs = new[] { "0" };
        private static readonly string[] HARD_CODE_SPORT_IDS = new[] { "-2" };
        private const string ProductResourceUrl = "api/productresource";
        private const int OthersProductId = 999;

        private static string LangCode => CultureInfo.CurrentUICulture.Name;

        private readonly IRemotingRefService _remotingRefService;
        private readonly ICommonApiClient _commonApiClient;

        public LangResourceService(IRemotingRefService remotingRefService, ICommonApiClient commonApiClient)
        {
            _remotingRefService = remotingRefService;
            _commonApiClient = commonApiClient;
        }

        public string GetName(string type, string id, string defaultValue)
        {
            if (type.Equals(TranslationConst.BettypeType, StringComparison.OrdinalIgnoreCase) && !Array.Exists(EXCEPTION_BETTYPE_IDs, _ => _.Equals(id)))
            {
                return GetDisplayBetTypeName(id, defaultValue);
            }

            if (type.Equals(TranslationConst.SportType, StringComparison.OrdinalIgnoreCase) && !Array.Exists(EXCEPTION_SPORT_IDs, _ => _.Equals(id)))
            {
                return GetSportName(id, defaultValue);
            }

            if (type.Equals(TranslationConst.ProductType, StringComparison.OrdinalIgnoreCase))
            {
                return GetProductName(id, defaultValue);
            }

            var result = GetValue(type, id);
            result = string.IsNullOrEmpty(result) ? defaultValue : result;

            return result;
        }

        private string GetDisplayBetTypeName(string betTypeId, string defaultValue)
        {
            var result = _remotingRefService.GetDisplayedBetTypeName(betTypeId, LangCode);
            if (string.IsNullOrEmpty(result))
            {
                result = defaultValue;
            }
            return result;
        }

        private string GetProductName(string productId, string defaultValue)
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;

            Int32.TryParse(productId, out int parsedProductId);
            parsedProductId = parsedProductId == 0 ? OthersProductId : parsedProductId - 1;

            var result = _commonApiClient.GetAsync<string>(
               ProductResourceUrl + $"/{parsedProductId}",
               currentCulture.Name).ConfigureAwait(false).GetAwaiter().GetResult();

            if (string.IsNullOrEmpty(result))
            {
                result = defaultValue;
            }
            return result;
        }

        private string GetSportName(string sportId, string defaultValue)
        {
            if (Array.Exists(HARD_CODE_SPORT_IDS, _ => _.Equals(sportId)))
            {
                return GetValue(TranslationConst.SportType, sportId);
            }

            var result = _remotingRefService.GetSportName(sportId, LangCode);
            if (string.IsNullOrEmpty(result))
            {
                result = defaultValue;
            }
            return result;
        }

        private static string GetValue(string type, string id)
        {
            var key = $"{type}_{id}";
            var result = ViewResource.ResourceManager.GetString(key);
            return result;
        }
    }
}