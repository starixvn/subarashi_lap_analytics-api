using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Summary.Models.ViewModels
{
    public class WidgetByPlatformViewModel
    {
        public IList<KeyValuePair<string, decimal>> Data { get; set; }

        public decimal Total { get; set; }
    }
}