namespace Fanex.Athena.AnalyticsAPI.Core.UnitTests.Service
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Services;
    using Xunit;

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
        public void AuthInfo_WhenCall_ReturnCorrectly()
        {
            //Arrange
            var expectedUserData = new
            {
                SiteName = "123",
                UserName = "username1",
                UserID = 1,
                Language = "en",
            };

            //Action
            _target = new AuthInfoService(_mockContextAccessor.Object);
            _mockHttpContext.Setup(_ => _.User).Returns(new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity(new[]
                {
                    new Claim("SiteName", expectedUserData.SiteName),
                    new Claim("UserSEQ", expectedUserData.UserID.ToString()),
                    new Claim("Language", expectedUserData.Language),
                })
            }));

            //Assert
            Assert.Equal(expectedUserData.UserID, _target.UserId);
            Assert.Equal(expectedUserData.SiteName, _target.SiteName);
            Assert.Equal(expectedUserData.Language, _target.Language);
        }
    }
}