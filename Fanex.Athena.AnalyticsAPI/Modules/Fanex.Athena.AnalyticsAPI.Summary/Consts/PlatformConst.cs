using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Summary.Consts
{
    public static class PlatformConst
    {
        public const int All = 0;

        public const int DesktopNewAsia = 1;

        public const int DesktopChina = 2;

        public const int DesktopOthers = 3;

        public const int MobileStandard = 4;

        public const int MobileEuro = 5;

        public const int MobileCompact = 6;

        public const int MobileOthers = 7;

        public const int ESportsDesktop = 8;

        public const int ESportsMobile = 9;

        public const int ESportsApp = 10;

        public const int MobileApp = 11;

        public const int NativeApp = 12;

        public const int Tablet = 13;

        public const int DeepLink = 14;

        public const int WAP = 15;

        public const int BetfromOthers = 88;

        public static readonly IReadOnlyDictionary<int, string> MapPlatformKeys = new Dictionary<int, string>
        {
            { All, nameof(All) },
            { DesktopNewAsia, nameof(DesktopNewAsia) },
            { DesktopChina, nameof(DesktopChina) },
            { DesktopOthers, nameof(DesktopOthers) },
            { MobileStandard, nameof(MobileStandard) },
            { MobileEuro, nameof(MobileEuro) },
            { MobileCompact, nameof(MobileCompact) },
            { MobileOthers, nameof(MobileOthers) },
            { ESportsDesktop, nameof(ESportsDesktop) },
            { ESportsMobile, nameof(ESportsMobile) },
            { ESportsApp, nameof(ESportsApp) },
            { MobileApp, nameof(MobileApp) },
            { NativeApp, nameof(NativeApp) },
            { Tablet, nameof(Tablet) },
            { DeepLink, nameof(DeepLink) },
            { WAP, nameof(WAP) },
            { BetfromOthers, nameof(BetfromOthers) }
        };
    }
}