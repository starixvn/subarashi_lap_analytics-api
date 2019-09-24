using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;
using System;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class MeasurementGrowthDTO
    {
        [SsasColumn("DimSite[Licensee Name]")]
        public string LicenseeName { get; set; }

        [SsasColumn("DimDate[MonthKey]")]
        public DateTime Time { get; set; }

        [SsasColumn("[Measurement]")]
        public decimal Total { get; set; }
    }
}