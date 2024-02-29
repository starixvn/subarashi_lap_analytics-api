namespace Fanex.Athena.AnalyticsAPI.Configuration
{
    public class IdentityServerConfig
    {
        public string Host { get; set; }

        public int MyProperty { get; set; }

        public CredentialsConfig CredentialClient { get; set; }

        public CredentialsConfig CredentialIntrospection { get; set; }

        public string Scope { get; set; }

        public string GrantType { get; set; }

        public string TokenEndpoint { get; set; }

        public int CacheDuration { get; set; }
    }

    public class CredentialsConfig
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}