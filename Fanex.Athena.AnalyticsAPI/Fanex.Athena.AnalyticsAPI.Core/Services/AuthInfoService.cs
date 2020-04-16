using Fanex.Athena.AnalyticsAPI.Core.Models;
using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using LAP.ONEBook.Security;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    public class AuthInfoService : IAuthInfoService
    {
        private readonly ClaimsPrincipal user;

        private DefaultAuthInfo<Permission, CurrencyAgentInfo> _authInfo;

        protected DefaultAuthInfo<Permission, CurrencyAgentInfo> AuthInfo
        {
            get
            {
                if (_authInfo == null)
                {
                    var userDataClaim = user.Claims.FirstOrDefault(_ => _.Type.Equals(ClaimTypes.UserData, StringComparison.OrdinalIgnoreCase));
                    _authInfo = JsonConvert.DeserializeObject<DefaultAuthInfo<Permission, CurrencyAgentInfo>>(userDataClaim.Value);
                }
                return _authInfo;
            }
        }

        public AuthInfoService(IHttpContextAccessor httpContextAccessor)
        {
            user = httpContextAccessor.HttpContext.User;
        }

        public string SiteName => user.Identity.Name;

        public string UserName => AuthInfo.UserID;

        public int UserId => AuthInfo.UserSEQ;

        public string Language => AuthInfo.Language;

        public string CurrencyIDList => string.Join(",", AuthInfo.Currency.Select(i => i.CurrencyId).ToArray());

        public IEnumerable<CurrencyAgentInfo> CurrencyAgentList => AuthInfo.Currency;
    }
}