using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;

namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    public class AuthInfoService : IAuthInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected HttpContext Context => _httpContextAccessor.HttpContext;
        protected ClaimsPrincipal User => Context.User;

        public AuthInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string SiteName => GetClaimValue("SiteName");

        public string UserName => GetClaimValue("UserID");

        public int UserId => GetClaimValue<int>("UserSEQ");

        public int SpecialPlatform => GetClaimValue<int>("SpecialPlatform");

        public int SpecialPlatformType => GetClaimValue<int>("SpecialPlatformType");

        public string Language => GetClaimValue("Language");

        public IEnumerable<string> OfferedSubProductList
        {
            get
            {
                var offeredSubProductList = GetClaimValue("OfferedSubProducts");
                return string.IsNullOrEmpty(offeredSubProductList) ? Enumerable.Empty<string>() : offeredSubProductList.Split(',');
            }
        }

        private IEnumerable<CurrencyAgentInfo> _currencyAgentList;

        public IEnumerable<CurrencyAgentInfo> CurrencyAgentList
        {
            get
            {
                if (_currencyAgentList is null)
                {
                    var agentClaims = User.Claims.Where(_ => _.Type.Equals("CurrencyAgent", StringComparison.OrdinalIgnoreCase));
                    _currencyAgentList = agentClaims.Select(agent => JsonConvert.DeserializeObject<CurrencyAgentInfo>(agent.Value));
                }

                return _currencyAgentList;
            }
        }

        private T GetClaimValue<T>(string claimType)
           => (T)Convert.ChangeType(GetClaimValue(claimType), typeof(T));

        private string GetClaimValue(string claimType)
            => User.FindFirst(claimType)?.Value;
    }
}