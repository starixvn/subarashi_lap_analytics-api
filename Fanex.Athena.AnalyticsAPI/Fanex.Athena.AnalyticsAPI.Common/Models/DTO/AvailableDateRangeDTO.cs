using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;
using System;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.DTO
{
    public class AvailableDateRangeDTO
    {
        private DateTime _fromDate;

        [SsasColumn("[FromDate]")]
        public DateTime FromDate
        {
            get
            {
                return _fromDate.ToLocalTime();
            }
            set
            {
                _fromDate = value;
            }
        }

        private DateTime _toDate;

        [SsasColumn("[ToDate]")]
        public DateTime ToDate
        {
            get
            {
                return _toDate.ToLocalTime();
            }
            set
            {
                _toDate = value;
            }
        }
    }
}