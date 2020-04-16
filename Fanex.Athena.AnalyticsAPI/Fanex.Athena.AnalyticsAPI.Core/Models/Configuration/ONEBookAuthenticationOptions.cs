using Microsoft.AspNetCore.Authentication;

namespace Fanex.Athena.AnalyticsAPI.Core.Models.Configuration
{
    public class ONEBookAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string SecurityConnectionString { get; set; }
    }
}