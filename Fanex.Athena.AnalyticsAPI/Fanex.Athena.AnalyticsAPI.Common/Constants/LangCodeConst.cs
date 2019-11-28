using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Constants
{
    public static class LangCodeConst
    {
        public const string EnUS = "en-US";
        public const string ZhTW = "zh-TW";
        public const string ZhCN = "zh-CN";
        public const string ZhHans = "zh-Hans";

        public static readonly IDictionary<string, string> Mapper = new Dictionary<string, string>
        {
            { "ch", ZhTW },
            { "cs", ZhCN },
            { "en", EnUS },
            { "zhcn", ZhHans }
        };
    }
}