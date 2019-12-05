using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Resources.Lang;
using Fanex.RemotingRef;
using System;
using System.Threading;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class LangResourceService : ILangResourceService
    {
        private readonly IRemotingRefService _remotingRefService;

        public LangResourceService(IRemotingRefService remotingRefService)
        {
            _remotingRefService = remotingRefService;
        }

        public string GetName(string type, string id, string defaultValue)
        {
            if (type.Equals(TranslationConst.BettypeType, StringComparison.OrdinalIgnoreCase))
            {
                return GetDisplayBetTypeName(id, defaultValue);
            }

            var result = GetValue(type, id);
            result = string.IsNullOrEmpty(result) ? defaultValue : result;

            return result;
        }

        private string GetDisplayBetTypeName(string betTypeId, string defaultValue)
        {
            var langCode = Thread.CurrentThread.CurrentCulture.Name;
            var result = _remotingRefService.GetDisplayedBetTypeName(betTypeId, langCode);
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