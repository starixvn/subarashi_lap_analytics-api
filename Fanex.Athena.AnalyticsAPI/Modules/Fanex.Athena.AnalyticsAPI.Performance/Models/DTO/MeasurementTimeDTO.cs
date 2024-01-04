using System;
using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class MeasurementTimeDTO : IMeasurement
    {
        [SsasColumn("DimDate[MonthKey]")]
        public DateTime Time { get; set; }

        [SsasColumn("[Measurement]")]
        public double Total { get; set; }
    }
}