namespace Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels
{
    using Helpers;

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