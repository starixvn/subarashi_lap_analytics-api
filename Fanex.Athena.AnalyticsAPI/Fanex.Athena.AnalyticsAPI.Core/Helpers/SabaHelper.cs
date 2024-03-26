using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;

namespace Fanex.Athena.AnalyticsAPI.Core.Helpers
{
    public static class SabaHelper
    {
        public static bool IsHideSaba(int specialPlatform)
        {
            if (specialPlatform == SpecialPlatform.Jollysports || specialPlatform == SpecialPlatform.JollysportCV)
            {
                return true;
            }
            return false;
        }
    }
}