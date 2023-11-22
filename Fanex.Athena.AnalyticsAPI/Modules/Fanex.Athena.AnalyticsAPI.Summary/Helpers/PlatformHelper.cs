using System.Collections.Generic;
using Fanex.Athena.AnalyticsAPI.Summary.Consts;

namespace Fanex.Athena.AnalyticsAPI.Summary.Helpers
{
    public static class PlatformHelper
    {
        private static readonly IDictionary<int, string> NormalPlatform = new Dictionary<int, string>
        {
            { PlatformConst.DesktopNewAsia, nameof(PlatformConst.DesktopNewAsia) },
            { PlatformConst.DesktopChina, nameof(PlatformConst.DesktopChina) },
            { PlatformConst.DesktopOthers, nameof(PlatformConst.DesktopOthers) },
            { PlatformConst.DesktopGalaxy, nameof(PlatformConst.DesktopGalaxy) },
            { PlatformConst.MobileStandard, nameof(PlatformConst.MobileStandard) },
            { PlatformConst.MobileEuro, nameof(PlatformConst.MobileEuro) },
            { PlatformConst.MobileGalaxy, nameof(PlatformConst.MobileGalaxy) },
            { PlatformConst.MobileCompact, nameof(PlatformConst.MobileCompact) },
            { PlatformConst.MobileOthers, nameof(PlatformConst.MobileOthers) },
            { PlatformConst.ESportsDesktop, nameof(PlatformConst.ESportsDesktop) },
            { PlatformConst.ESportsMobile, nameof(PlatformConst.ESportsMobile) },
            { PlatformConst.ESportsApp, nameof(PlatformConst.ESportsApp) },
            { PlatformConst.MobileApp, nameof(PlatformConst.MobileApp) },
            { PlatformConst.NativeApp, nameof(PlatformConst.NativeApp) },
            { PlatformConst.Tablet, nameof(PlatformConst.Tablet) },
            { PlatformConst.DeepLink, nameof(PlatformConst.DeepLink) },
            { PlatformConst.WAP, nameof(PlatformConst.WAP) },
            { PlatformConst.ChatRoom, nameof(PlatformConst.ChatRoom) },
            { PlatformConst.BetfromOthers, nameof(PlatformConst.BetfromOthers) }
        };

        private static readonly IDictionary<int, string> JollysportsPlatform = new Dictionary<int, string>
        {
            { PlatformConst.Desktop, nameof(PlatformConst.Desktop) },
            { PlatformConst.Mobile, nameof(PlatformConst.Mobile) },
            { PlatformConst.Others, nameof(PlatformConst.BetfromOthers) },
        };

        public static string GetPlatformKey(int type)
        {
            if (JollysportsPlatform.ContainsKey(type))
            {
                return JollysportsPlatform[type];
            }

            if (NormalPlatform.ContainsKey(type))
            {
                return NormalPlatform[type];
            }

            return "All";
        }
    }
}