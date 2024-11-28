using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Consts
{
    public static class CurrencyConst
    {
        public static readonly IEnumerable<string> SupportedBaseCurrencies = new[]
        {
            "EUR", "USD", "GBP", "MYR", "CNY", "THB", "IDR", "INH", "BDT", "INR", "PHP", "USDT", "MP", "NPR", "TRY", "JPY", "MXN", "NGN", "BRL"
        };
    }
}