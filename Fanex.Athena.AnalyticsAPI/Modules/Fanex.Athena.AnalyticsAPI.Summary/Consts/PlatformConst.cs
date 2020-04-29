using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Summary.Consts
{
    public static class PlatformConst
    {
        public const int All = 0;

        public const int Asia = 1;

        public const int China = 2;

        public const int Euro = 3;

        public const int Wap = 4;

        public const int Mobile = 5;

        public const int Tablet = 6;

        public const int NewAsia = 7;

        public const int MobileApp = 8;

        public const int MobileLiteVersion = 9;

        public const int DeepLink = 10;

        public const int NativeApp = 11;

        public const int ESportsDesktop = 12;

        public const int ESportsMobile = 13;

        public const int ESportsApp = 14;

        public const int BetfromOthers = 88;

        public static readonly IDictionary<int, string> MapPlatformKeys = new Dictionary<int, string>
        {
            { All, nameof(All) },
            { Asia, nameof(Asia) },
            { China, nameof(China) },
            { Euro, nameof(Euro) },
            { Wap, nameof(Wap) },
            { Mobile, nameof(Mobile) },
            { Tablet, nameof(Tablet) },
            { NewAsia, nameof(NewAsia) },
            { MobileApp, nameof(MobileApp) },
            { MobileLiteVersion, nameof(MobileLiteVersion) },
            { DeepLink, nameof(DeepLink) },
            { BetfromOthers, nameof(BetfromOthers) },
            { NativeApp, nameof(NativeApp) },
            { ESportsDesktop, nameof(ESportsDesktop) },
            { ESportsMobile, nameof(ESportsMobile) },
            { ESportsApp, nameof(ESportsApp) }
        };
    }
}