namespace Fanex.Athena.AnalyticsAPI.Common.Helpers
{
    using System;
    using System.Globalization;

    public static class DateTimeHelper
    {
        private const int NUMBER_OF_MONTHS_OF_YEAR = 12;

        public const string DATE_FORMAT = "MM/dd/yyyy";
        public const string MONTH_FORMAT = "MM/yyyy";

        private static IFormatProvider CULTURE_INFO_DEFAULT = new CultureInfo("en-US");

        private static readonly TimeZoneInfo DatabaseServerTimezoneInfo = TimeZoneInfo.CreateCustomTimeZone(
                    "DB Server Time",
                    TimeSpan.FromHours(-4),
                    "(GMT-04:00) DB Server Time",
                    "DB Server Time");

        public static Func<DateTimeOffset> DateTimeOffsetProvider { get; set; } = () => DateTimeOffset.Now;

        public static DateTime Now => DateTimeOffsetProvider().DateTime;

        public static DateTime UtcNow => DateTimeOffsetProvider().UtcDateTime;

        public static DateTime CreateDateTime(int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
            => new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.Zero).DateTime;

        public static DateTime DatabaseServerNow =>
            TimeZoneInfo.ConvertTimeFromUtc(
                UtcNow,
                DatabaseServerTimezoneInfo);

        public static string ParseFromFormatToString(string date, bool includeEndOfMonth)
        {
            if (IsMonthFormat(date))
            {
                return ParseFromFormatMonthToDate(date, includeEndOfMonth).ToString(DATE_FORMAT);
            }

            return ParseFromFormatDateToDate(date, includeEndOfMonth).ToString(DATE_FORMAT);
        }

        public static DateTime ParseFromFormatMonthToDate(string date, bool includeEndOfMonth)
        {
            var parsedDate = DateTime.ParseExact(date, MONTH_FORMAT, CULTURE_INFO_DEFAULT);
            var day = includeEndOfMonth ? DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month) : 1;

            return CreateDateTime(parsedDate.Year, parsedDate.Month, day);
        }

        public static DateTime ParseFromFormatDateToDate(string date, bool includeEndOfMonth)
        {
            var parsedDate = DateTime.ParseExact(date, DATE_FORMAT, CULTURE_INFO_DEFAULT);
            var day = includeEndOfMonth ? DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month) : 1;

            return CreateDateTime(parsedDate.Year, parsedDate.Month, day);
        }

        public static DateTime ParseFromFormatDateToDate(string date)
        {
            var parsedDate = DateTime.ParseExact(date, DATE_FORMAT, CULTURE_INFO_DEFAULT);
            return parsedDate;
        }

        public static int GetDistanceBetweenTwoDate(DateTime from, DateTime to)
        {
            return NUMBER_OF_MONTHS_OF_YEAR * (to.Year - from.Year) + (to.Month - from.Month);
        }

        private static bool IsMonthFormat(string date)
        {
            return DateTime.TryParseExact(date, MONTH_FORMAT,
                CULTURE_INFO_DEFAULT,
                DateTimeStyles.None, out DateTime dt);
        }
    }
}