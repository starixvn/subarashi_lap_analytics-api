namespace Fanex.Athena.AnalyticsAPI.Common.Helpers
{
    using System;

    public static class CurrencyHelper
    {
        public static string GetBasedCurrency(string basedCurrency)
        {
            if (IsEqual(basedCurrency, "MP"))
            {
                return "PHP";
            }
            return basedCurrency;
        }

        private static bool IsEqual(string a, string b)
        {
            return string.Equals(a, b, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}