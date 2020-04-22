using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.Models;
using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using LAP.ONEBook.Security;
using LAP.ONEBook.Security.StorageProvider.SqlServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Core.ONEBookAuth
{
    public class PostConfigureONEBookAuthenticationOptions : IPostConfigureOptions<ONEBookAuthenticationOptions>
    {
        public void PostConfigure(string name, ONEBookAuthenticationOptions options)
        {
            options.Validate();
        }
    }

    public class ONEBookAuthenticationOptions : AuthenticationSchemeOptions
    {
        public bool AllowBypass { get; set; }

        public Func<object> FakeData { get; set; }

        public Lazy<object> LazyFakeData => new Lazy<object>(FakeData);

        private string _securityConnectionString;

        public string SecurityConnectionString
        {
            get { return _securityConnectionString; }
            set
            {
                _securityConnectionString = value;
                if (AuthClient == null && !string.IsNullOrEmpty(value))
                {
                    AuthClient = new Lazy<IAuth<DefaultAuthInfo<Permission, CurrencyAgentInfo>>>(
                        () => new Auth<DefaultAuthInfo<Permission, CurrencyAgentInfo>>(new SqlServerProvider(value)));
                }
            }
        }

        public Lazy<IAuth<DefaultAuthInfo<Permission, CurrencyAgentInfo>>> AuthClient { get; private set; }

        public Func<HttpRequest, string> TokenRetrieval { get; set; } = AccessTokenFromDefault;

        public override void Validate()
        {
            base.Validate();

            if (string.IsNullOrEmpty(SecurityConnectionString))
            {
                throw new Exception($"{nameof(SecurityConnectionString)} is required");
            }

            if (AuthClient == null)
            {
                throw new Exception($"{nameof(AuthClient)} is required");
            }

            if (TokenRetrieval == null)
            {
                throw new Exception($"{nameof(TokenRetrieval)} is required");
            }
        }

        private static Func<HttpRequest, string> AccessTokenFromDefault { get; set; } = request =>
        {
            string token;
            if (request.Query.TryGetValue(AuthConst.AUTH_KEY, out StringValues queryToken))
            {
                token = queryToken.FirstOrDefault();
            }
            else if (request.Headers.TryGetValue(AuthConst.AUTH_KEY, out StringValues headerToken))
            {
                token = headerToken.FirstOrDefault();
            }
            else
            {
                token = null;
            }

            return token;
        };
    }
}