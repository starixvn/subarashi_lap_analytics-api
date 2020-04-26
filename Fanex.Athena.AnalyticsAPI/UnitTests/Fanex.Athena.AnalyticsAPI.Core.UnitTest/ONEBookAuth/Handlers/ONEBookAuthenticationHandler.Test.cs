using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.Models;
using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using Fanex.Athena.AnalyticsAPI.Core.ONEBookAuth;
using Fanex.Athena.AnalyticsAPI.UnitTests.Utils.Helpers;
using FluentAssertions;
using LAP.ONEBook.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Core.UnitTest.ONEBookAuth
{
    public class ONEBookAuthenticationHandler_Test
    {
        private readonly ONEBookAuthenticationHandler _target;
        private readonly Mock<IOptionsMonitor<ONEBookAuthenticationOptions>> _mockOptions;
        private readonly Mock<IAuth<DefaultAuthInfo<Permission, CurrencyAgentInfo>>> _mockAuthClient;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<HttpRequest> _mockRequest;

        public ONEBookAuthenticationHandler_Test()
        {
            _mockHttpContext = new Mock<HttpContext>();
            _mockRequest = new Mock<HttpRequest>();
            _mockHttpContext.Setup(_ => _.Request).Returns(_mockRequest.Object);
            _mockAuthClient = new Mock<IAuth<DefaultAuthInfo<Permission, CurrencyAgentInfo>>>();

            _mockOptions = new Mock<IOptionsMonitor<ONEBookAuthenticationOptions>>();
            var logger = new Mock<ILogger<ONEBookAuthenticationHandler>>();
            var _loggerFactory = new Mock<ILoggerFactory>();
            _loggerFactory.Setup(x => x.CreateLogger(It.IsAny<String>())).Returns(logger.Object);

            _target = new ONEBookAuthenticationHandler(_mockOptions.Object,
                _loggerFactory.Object,
                Mock.Of<UrlEncoder>(),
                Mock.Of<ISystemClock>());
        }

        [Fact]
        public async Task AuthenticateAsync_AllowBypass_UseFakeData()
        {
            //Arrange
            var fakeUserData = FakeHelper.Create<DefaultAuthInfo<Permission, CurrencyAgentInfo>>();

            var options = FakeOptions(true, fakeUserData);
            _mockOptions.Setup(_ => _.Get(AuthConst.ONEBookScheme)).Returns(options);
            await _target.InitializeAsync(new AuthenticationScheme(AuthConst.ONEBookScheme, null, typeof(ONEBookAuthenticationHandler)), Mock.Of<HttpContext>());

            //Act
            var result = await _target.AuthenticateAsync();

            //Assert
            Assert.True(result.Succeeded);
            AssertAuthenticationResult(fakeUserData, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task AuthenticateAsync_AuthTokenIsEmptyOrNull_AuthenticationFail(string token)
        {
            //Arrange

            _mockRequest.Setup(_ => _.Query).Returns(new QueryCollection(new Dictionary<string, StringValues> { { AuthConst.AUTH_KEY, new StringValues(token) } }));
            _mockRequest.Setup(_ => _.Headers).Returns(new HeaderDictionary { { AuthConst.AUTH_KEY, new StringValues(token) } });

            var options = FakeOptions(false);
            _mockOptions.Setup(_ => _.Get(AuthConst.ONEBookScheme)).Returns(options);

            await _target.InitializeAsync(new AuthenticationScheme(AuthConst.ONEBookScheme, null, typeof(ONEBookAuthenticationHandler)), _mockHttpContext.Object);

            //Act
            var result = await _target.AuthenticateAsync();

            //Assert
            Assert.False(result.Succeeded);
            AuthenticateResult.NoResult().Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task AuthenticateAsync_TokenInvalid_AuthenticationFail()
        {
            //Arrange
            var token = FakeHelper.Create<string>();
            var fakeUserData = new DefaultAuthInfo<Permission, CurrencyAgentInfo>
            {
                SiteName = FakeHelper.Create<string>(),
                UserName = FakeHelper.Create<string>(),
                UserSEQ = FakeHelper.Create<int>(),
                ActionLink = FakeHelper.Create<string>()
            };

            _mockRequest.Setup(_ => _.Query).Returns(new QueryCollection(new Dictionary<string, StringValues>()));
            _mockRequest.Setup(_ => _.Headers).Returns(new HeaderDictionary { { AuthConst.AUTH_KEY, new StringValues(token) } });
            _mockAuthClient.Setup(_ => _.AuthenticateAsync(token)).Returns(Task.FromResult(fakeUserData));

            var options = FakeOptions(false);
            _mockOptions.Setup(_ => _.Get(AuthConst.ONEBookScheme)).Returns(options);

            await _target.InitializeAsync(new AuthenticationScheme(AuthConst.ONEBookScheme, null, typeof(ONEBookAuthenticationHandler)), _mockHttpContext.Object);

            //Act
            var result = await _target.AuthenticateAsync();

            //Assert
            Assert.False(result.Succeeded);
            AuthenticateResult.NoResult().Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task AuthenticateAsync_TokenValid_AuthenticationSuccess()
        {
            //Arrange
            var token = FakeHelper.Create<string>();
            var fakeUserData = new DefaultAuthInfo<Permission, CurrencyAgentInfo>
            {
                IsAdminstrator = FakeHelper.Create<bool>(),
                SiteName = FakeHelper.Create<string>(),
                UserName = FakeHelper.Create<string>(),
                UserSEQ = FakeHelper.Create<int>(),
                Permissions = FakeHelper.CreateMany<Permission>(),
                ActionLink = "/" + FakeHelper.Create<string>()
            };

            _mockRequest.Setup(_ => _.Query).Returns(new QueryCollection(new Dictionary<string, StringValues>()));
            _mockRequest.Setup(_ => _.Headers).Returns(new HeaderDictionary { { AuthConst.AUTH_KEY, new StringValues(token) } });
            _mockRequest.Setup(_ => _.Path).Returns(new PathString(fakeUserData.ActionLink));

            _mockAuthClient.Setup(_ => _.AuthenticateAsync(token)).Returns(Task.FromResult(fakeUserData));

            var options = FakeOptions(false);
            _mockOptions.Setup(_ => _.Get(AuthConst.ONEBookScheme)).Returns(options);

            await _target.InitializeAsync(new AuthenticationScheme(AuthConst.ONEBookScheme, null, typeof(ONEBookAuthenticationHandler)), _mockHttpContext.Object);

            //Act
            var result = await _target.AuthenticateAsync();

            //Assert
            Assert.True(result.Succeeded);
            AssertAuthenticationResult(fakeUserData, result);
        }

        private void AssertAuthenticationResult(DefaultAuthInfo<Permission, CurrencyAgentInfo> fakeUserData, AuthenticateResult result)
        {
            Assert.Equal(fakeUserData.SiteName, result.Principal.Identity.Name);

            Assert.Equal(JsonConvert.SerializeObject(fakeUserData),
                result.Principal.Claims.FirstOrDefault(_ => _.Type.Equals(ClaimTypes.UserData))?.Value);

            (fakeUserData.IsAdminstrator ? new[] { PermissionConst.SUPER_ADMIN } :
                        fakeUserData.Permissions.Select(_ => _.FunctionKey).ToArray())
                        .Should().BeEquivalentTo(
                result.Principal.Claims.Where(_ => _.Type.Equals(ClaimTypes.Role)).Select(_ => _.Value));
        }

        private ONEBookAuthenticationOptions FakeOptions(bool allowBypass, object fakeUserData = null)
        {
            var options = new ONEBookAuthenticationOptions();
            options.AllowBypass = allowBypass;
            options.FakeData = () => fakeUserData;
            if (!allowBypass)
            {
                options.AuthClient = new Lazy<IAuth<DefaultAuthInfo<Permission, CurrencyAgentInfo>>>(() => _mockAuthClient.Object);
                options.SecurityConnectionString = FakeHelper.Create<string>();
            }

            return options;
        }
    }
}