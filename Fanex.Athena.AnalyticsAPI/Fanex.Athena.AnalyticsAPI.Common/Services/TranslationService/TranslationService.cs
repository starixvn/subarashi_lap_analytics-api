using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Resources.Lang;
using Fanex.RemotingRef;
using System.Collections.Generic;
using System.Resources;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class TranslationService : ITranslationService
    {
        private static IDictionary<string, ResourceManager> resourceFactory = new Dictionary<string, ResourceManager>
        {
            { LangCodeConst.EnUS, LangEnUS.ResourceManager },
            { LangCodeConst.ZhCN, LangZhCN.ResourceManager },
            { LangCodeConst.ZhHans, LangZhHans.ResourceManager },
            { LangCodeConst.ZhTW, LangZhTW.ResourceManager }
        };

        private readonly IRemotingRefService _remotingRefService;

        public TranslationService(IRemotingRefService remotingRefService)
        {
            _remotingRefService = remotingRefService;
        }

        public string GetDisplayBetTypeName(string betTypeId, string langCode, string defaultValue)
        {
            var result = _remotingRefService.GetDisplayedBetTypeName(betTypeId, langCode);
            if (string.IsNullOrWhiteSpace(result))
            {
                result = defaultValue;
            }
            return result;
        }

        public string GetPlatformName(string platformId, string langCode, string defaultValue)
        {
            return GetValue("platform", platformId, langCode) ?? defaultValue;
        }

        public string GetProductName(string productId, string langCode, string defaultValue)
        {
            return GetValue("product", productId, langCode) ?? defaultValue;
        }

        private static string GetValue(string type, string id, string langCode)
        {
            if (!resourceFactory.ContainsKey(langCode))
            {
                return null;
            }

            var key = GetKey(type, id);
            var result = resourceFactory[langCode].GetString(key);

            return result;
        }

        private static string GetKey(string type, string id)
        {
            return $"{type}_{id}";
        }
    }
}