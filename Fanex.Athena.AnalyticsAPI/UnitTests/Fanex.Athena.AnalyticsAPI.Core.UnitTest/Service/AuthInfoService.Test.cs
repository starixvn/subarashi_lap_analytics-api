using Fanex.Athena.AnalyticsAPI.Core.Models;
using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using Fanex.Athena.AnalyticsAPI.Core.Services;
using Fanex.Athena.AnalyticsAPI.UnitTests.Utils.Helpers;
using FluentAssertions;
using LAP.ONEBook.Security;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Core.UnitTest.Service
{
    public class AuthInfoService_Test
    {
        private AuthInfoService _target;

        private readonly Mock<IHttpContextAccessor> _mockContextAccessor;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<ClaimsPrincipal> _mockUser;

        public AuthInfoService_Test()
        {
            _mockContextAccessor = new Mock<IHttpContextAccessor>();
            _mockHttpContext = new Mock<HttpContext>();
            _mockUser = new Mock<ClaimsPrincipal>();
            _mockHttpContext.Setup(_ => _.User).Returns(_mockUser.Object);
            _mockContextAccessor.Setup(_ => _.HttpContext).Returns(_mockHttpContext.Object);
        }

        [Fact]
        public void SiteName_WhenCall_ReturnCorrectly()
        {
            //Arrange
            var expectedSiteName = FakeHelper.Create<string>();

            var identity = new Mock<IIdentity>();
            identity.Setup(_ => _.Name).Returns(expectedSiteName);

            _mockUser.Setup(_ => _.Identity).Returns(identity.Object);

            //Action
            _target = new AuthInfoService(_mockContextAccessor.Object);

            //Assert
            Assert.Equal(expectedSiteName, _target.SiteName);
        }

        [Fact]
        public void AuthInfo_WhenCall_ReturnCorrectly()
        {
            //Arrange
            var expectedUserData = FakeHelper.Create<DefaultAuthInfo<Permission, CurrencyAgentInfo>>();
            var fakeClaims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(expectedUserData))
            };

            _mockUser.Setup(_ => _.Claims).Returns(fakeClaims);

            //Action
            _target = new AuthInfoService(_mockContextAccessor.Object);

            //Assert
            Assert.Equal(expectedUserData.UserID, _target.UserName);
            Assert.Equal(expectedUserData.UserSEQ, _target.UserId);
            Assert.Equal(expectedUserData.Language, _target.Language);
            expectedUserData.Currency.Should().BeEquivalentTo(_target.CurrencyAgentList);
        }
    }
}