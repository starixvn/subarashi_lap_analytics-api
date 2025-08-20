namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    using System;
    using Common.Attributes;

    public class MeasurementTimeDTO : IMeasurement
    {
        [SsasColumn("DimDate[MonthKey]")]
        public DateTime Time { get; set; }

        [SsasColumn("[Measurement]")]
        public double Total { get; set; }
    }
}