namespace Fanex.Athena.ExtService.Common.Configuration
{
    public class LoggingSetting
    {
        public string Category { get; set; }
        public SentryEngine SentryEngine { get; set; }
        public RabbitEngine RabbitEngine { get; set; }
    }

    public class SentryEngine
    {
        public string BackendDsn { get; set; }
    }

    public class RabbitEngine
    {
        public string ConnectionString { get; set; }
    }
}