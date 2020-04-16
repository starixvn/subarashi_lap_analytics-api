using Fanex.Athena.AnalyticsAPI.Summary.Consts;

namespace Fanex.Athena.AnalyticsAPI.Summary.Helpers
{
    public static class PlatformHelper
    {
        public static string GetPlatformKey(int type)
        {
            return PlatformConst.MapPlatformKeys.ContainsKey(type) ? PlatformConst.MapPlatformKeys[type] : "All";
        }
    }
}