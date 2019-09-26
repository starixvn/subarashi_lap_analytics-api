using Fanex.Athena.AnalyticsAPI.Common.Helpers;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels
{
    public class MonthRangeMetaModel
    {
        private string _fromMonth;

        public string FromMonth
        {
            get
            {
                return DateTimeHelper.ParseFromFormatMonthToString(_fromMonth, false);
            }
            set
            {
                _fromMonth = value;
            }
        }

        private string _toMonth;

        public string ToMonth
        {
            get
            {
                return DateTimeHelper.ParseFromFormatMonthToString(_toMonth, true);
            }
            set
            {
                _toMonth = value;
            }
        }
    }
}