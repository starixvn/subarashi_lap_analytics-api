using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.DTO
{
    public class AnalyticResult<T>
    {
        public IEnumerable<ChartItemResult<T>> Charts { get; set; }
    }

    public class ChartItemResult<T>
    {
        public int Type { get; set; }
        public string Name { get; set; }
        public IEnumerable<T> ChartColumns { get; set; }
        public int Order { get; set; }
        public string ExtraInfo { get; set; }
    }

    public class ChartColumnResult<T>
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public IEnumerable<T> PartChartColumns { get; set; }
    }

    public class PartChartColumnResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public int Order { get; set; }
    }
}