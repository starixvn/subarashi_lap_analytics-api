using Fanex.Athena.AnalyticsAPI.Core.Models.Configuration;
using LAP.ONEBook.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    public class AuthService : IAuthService
    {
        private const string DEFAULT_SITE_NAME = "12Bet"; //only development

        private readonly IAuth<DefaultAuthInfo<dynamic, dynamic>> _oneBookAuth;
        private readonly JwtSetting _jwtSetting;
        private readonly IHostingEnvironment _env;

        public AuthService(
            IHostingEnvironment env,
            IAuth<DefaultAuthInfo<dynamic, dynamic>> oneBookAuth,
            IOptions<JwtSetting> jwtSetting)
        {
            _env = env;
            _oneBookAuth = oneBookAuth;
            _jwtSetting = jwtSetting.Value;
        }

        public async Task<string> Authenticate(string lapToken)
        {
            var authInfo = _env.IsDevelopment() ? new DefaultAuthInfo<dynamic, dynamic>
            {
                SiteName = DEFAULT_SITE_NAME,
                UserName = DEFAULT_SITE_NAME,
                UserSEQ = 1
            } : await _oneBookAuth.AuthenticateAsync(lapToken).ConfigureAwait(true);

            return ValidateToken(authInfo) ? GenerateJwt(authInfo, _jwtSetting) : null;
        }

        private static string GenerateJwt(DefaultAuthInfo<dynamic, dynamic> authInfo, JwtSetting jwtSetting)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(jwtSetting.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtSetting.Issuer,
                Audience = jwtSetting.Issuer,
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim(ClaimTypes.Name, authInfo.SiteName)
                    }),
                Expires = DateTime.UtcNow.AddDays(jwtSetting.ExpireTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            var jwtStringFormat = tokenHandler.WriteToken(jwt);

            return jwtStringFormat;
        }

        private static bool ValidateToken(DefaultAuthInfo<dynamic, dynamic> authInfo)
        {
            if (string.IsNullOrEmpty(authInfo?.SiteName) || string.IsNullOrEmpty(authInfo?.UserName) || authInfo?.UserSEQ <= 0)
            {
                return false;
            }
            return true;
        }
    }
}