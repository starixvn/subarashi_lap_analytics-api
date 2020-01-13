namespace Fanex.Athena.AnalyticsAPI.Core.Models.Configuration
{
    public class JwtSetting
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public int ExpireTime { get; set; }
    }
}