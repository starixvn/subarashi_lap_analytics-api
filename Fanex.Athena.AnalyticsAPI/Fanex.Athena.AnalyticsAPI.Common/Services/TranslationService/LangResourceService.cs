using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Resources.Lang;
using Fanex.RemotingRef;
using System;
using System.Linq;
using System.Threading;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class LangResourceService : ILangResourceService
    {
        private static readonly string[] EXCEPTION_BETTYPE_IDs = new[] { "0" };
        private static readonly string[] EXCEPTION_SPORT_IDs = new[] { "0" };
        private static readonly string[] HARD_CODE_SPORT_IDS = new[] { "-2" }; 

        private static string LangCode => Thread.CurrentThread.CurrentCulture.Name;

        private readonly IRemotingRefService _remotingRefService;

        public LangResourceService(IRemotingRefService remotingRefService)
        {
            _remotingRefService = remotingRefService;
        }

        public string GetName(string type, string id, string defaultValue)
        {
            if (type.Equals(TranslationConst.BettypeType, StringComparison.OrdinalIgnoreCase) && !EXCEPTION_BETTYPE_IDs.Any(_ => _.Equals(id)))
            {
                return GetDisplayBetTypeName(id, defaultValue);
            }

            if (type.Equals(TranslationConst.SportType, StringComparison.OrdinalIgnoreCase) && !EXCEPTION_SPORT_IDs.Any(_ => _.Equals(id)))
            {
                return GetSportName(id, defaultValue);
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

        private string GetSportName(string sportId, string defaultValue)
        {
            if(HARD_CODE_SPORT_IDS.Any(_ => _.Equals(sportId)))
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