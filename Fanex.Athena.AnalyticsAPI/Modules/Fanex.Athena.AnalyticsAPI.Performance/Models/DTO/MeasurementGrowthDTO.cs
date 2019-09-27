using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;
using System;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class MeasurementGrowthDTO
    {
        [SsasColumn("DimDate[MonthKey]")]
        public DateTime Time { get; set; }

        [SsasColumn("[Measurement]")]
        public double Total { get; set; }
    }
}