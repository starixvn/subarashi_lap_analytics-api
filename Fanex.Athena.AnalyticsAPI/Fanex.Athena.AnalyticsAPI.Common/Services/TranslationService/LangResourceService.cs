using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Resources.Lang;
using Fanex.RemotingRef;
using System;
using System.Collections.Generic;
using System.Resources;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class LangResourceService : ILangResourceService
    {
        private static IDictionary<string, ResourceManager> resourceFactory = new Dictionary<string, ResourceManager>
        {
            { TranslationConst.EnUS, LangEnUS.ResourceManager },
            { TranslationConst.ZhCN, LangZhCN.ResourceManager },
            { TranslationConst.ZhHans, LangZhHans.ResourceManager },
            { TranslationConst.ZhTW, LangZhTW.ResourceManager }
        };

        private readonly IRemotingRefService _remotingRefService;

        public LangResourceService(IRemotingRefService remotingRefService)
        {
            _remotingRefService = remotingRefService;
        }

        public string GetName(string type, string id, string langCode, string defaultValue)
        {
            if (type.Equals(TranslationConst.BettypeType, StringComparison.OrdinalIgnoreCase))
            {
                return GetDisplayBetTypeName(id, langCode, defaultValue);
            }
            return GetValue(type, id, langCode) ?? defaultValue;
        }

        private string GetDisplayBetTypeName(string betTypeId, string langCode, string defaultValue)
        {
            var result = _remotingRefService.GetDisplayedBetTypeName(betTypeId, langCode);
            if (string.IsNullOrWhiteSpace(result))
            {
                result = defaultValue;
            }
            return result;
        }

        private static string GetValue(string type, string id, string langCode)
        {
            if (!resourceFactory.ContainsKey(langCode))
            {
                return null;
            }

            var key = $"{type}_{id}";
            var result = resourceFactory[langCode].GetString(key);

            return result;
        }
    }
}