using System;
using System.Globalization;

namespace Fanex.Athena.AnalyticsAPI.Common.Helpers
{
    public static class DateTimeHelper
    {
        public const string DATE_FORMAT = "MM/dd/yyyy";
        public const string MONTH_FORMAT = "MM/yyyy";

        public static string ParseToFullDateString(string date, bool includeEndOfMonth) //- MM/yyyy => MM/dd/yyyy
        {
            return ParseToFullDate(date, includeEndOfMonth).ToString(DATE_FORMAT);
        }

        public static DateTime ParseToFullDate(string date, bool includeEndOfMonth) //- MM/yyyy => MM/dd/yyyy
        {
            var parsedDate = DateTime.ParseExact(date, MONTH_FORMAT, CultureInfo.InvariantCulture);
            var day = includeEndOfMonth ? DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month) : 1;

            return new DateTime(parsedDate.Year, parsedDate.Month, day);
        }

        public static DateTime MinusMonth(this DateTime date, int numberOfMonth)
        {
            return date.AddMonths(-1);
        }
    }
}