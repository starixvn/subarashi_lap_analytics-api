using System;
using System.Globalization;

namespace Fanex.Athena.AnalyticsAPI.Common.Helpers
{
    public static class DateTimeHelper
    {
        public const string DATE_FORMAT = "MM/dd/yyyy";
        public const string MONTH_FORMAT = "MM/yyyy";

        public static string ParseFromFormatMonthToString(string date, bool includeEndOfMonth)
        {
            return ParseFromFormatMonthToDate(date, includeEndOfMonth).ToString(DATE_FORMAT);
        }

        public static DateTime ParseFromFormatMonthToDate(string date, bool includeEndOfMonth)
        {
            var parsedDate = DateTime.ParseExact(date, MONTH_FORMAT, CultureInfo.InvariantCulture);
            var day = includeEndOfMonth ? DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month) : 1;

            return new DateTime(parsedDate.Year, parsedDate.Month, day);
        }

        public static DateTime ParseFromFormatDateToDate(string date)
        {
            var parsedDate = DateTime.ParseExact(date, DATE_FORMAT, CultureInfo.InvariantCulture);
            return parsedDate;
        }
    }
}