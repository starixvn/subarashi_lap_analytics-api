using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Constants
{
    public static class TranslationConst
    {
        //lang code
        public const string EnUS = "en-US";
        public const string ZhTW = "zh-TW";
        public const string ZhCN = "zh-CN";
        public const string ZhHans = "zh-Hans";

        //tranlation type
        public const string NormalType = "normal";
        public const string ProductType = "product";
        public const string CurrencyType = "currency";
        public const string PlatformType = "platform";
        public const string BettypeType = "bettype";

        public static readonly IDictionary<string, string> LangCodeMapper = new Dictionary<string, string>
        {
            { "ch", ZhTW },
            { "cs", ZhCN },
            { "en", EnUS },
            { "zhcn", ZhHans }
        };
    }
}