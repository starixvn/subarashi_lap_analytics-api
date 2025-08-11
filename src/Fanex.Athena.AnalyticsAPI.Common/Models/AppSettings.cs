namespace Fanex.Athena.AnalyticsAPI.Common.Models
{
    public class AppSettings
    {
        public CommonApiHostConfig CommonApiHost { get; set; }
    }

    public class CommonApiHostConfig
    {
        public string Host { get; set; }
    }
}