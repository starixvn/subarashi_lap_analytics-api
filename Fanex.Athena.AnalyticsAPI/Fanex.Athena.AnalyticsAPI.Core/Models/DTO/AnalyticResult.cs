using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Core.Models.DTO
{
    public class AnalyticResult<T>
    {
        public IEnumerable<AnalyticResultItem<T>> Items { get; set; }
    }

    public class AnalyticResultItem<T>
    {
        public int Type { get; set; }
        public IEnumerable<T> Items { get; set; }
        public int Order { get; set; }
        public string ExtraInfo { get; set; }
    }

    public class PieceResult
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public int Order { get; set; }
    }

    public class TotalResultItem<T>
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}