namespace Fanex.Athena.AnalyticsAPI.Summary.Models.ViewModels
{
    using System.Collections.Generic;

    public class WidgetByPlatformViewModel
    {
        public IList<KeyValuePair<string, decimal>> Data { get; set; }

        public decimal Total { get; set; }
    }
}