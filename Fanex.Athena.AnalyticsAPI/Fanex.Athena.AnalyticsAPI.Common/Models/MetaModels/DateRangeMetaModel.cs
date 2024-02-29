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
                return _fromMonth;
            }

            set
            {
                _fromMonth = DateTimeHelper.ParseFromFormatToString(value, false);
            }
        }

        private string _toMonth;

        public string ToMonth
        {
            get
            {
                return _toMonth;
            }

            set
            {
                _toMonth = DateTimeHelper.ParseFromFormatToString(value, true);
            }
        }
    }
}