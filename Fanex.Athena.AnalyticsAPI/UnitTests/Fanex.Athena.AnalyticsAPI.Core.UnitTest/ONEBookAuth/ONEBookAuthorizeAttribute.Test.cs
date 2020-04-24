using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.ONEBookAuth;
using Fanex.Athena.AnalyticsAPI.UnitTests.Utils.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Core.UnitTest.ONEBookAuth
{
    public class ONEBookAuthorizeAttribute_Test
    {
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<ClaimsPrincipal> _mockUser;
        private readonly Mock<IIdentity> _mockIdentity;

        private readonly AuthorizationFilterContext _actionContext;
        private readonly ONEBookAuthorizeAttribute _target;

        public ONEBookAuthorizeAttribute_Test()
        {
            _mockHttpContext = new Mock<HttpContext>();
            _mockUser = new Mock<ClaimsPrincipal>();
            _mockIdentity = new Mock<IIdentity>();

            _mockHttpContext.Setup(_ => _.User).Returns(_mockUser.Object);
            _mockUser.Setup(_ => _.Identity).Returns(_mockIdentity.Object);

            var actionContext = new ActionContext(
                 _mockHttpContext.Object,
                 Mock.Of<RouteData>(),
                 Mock.Of<ActionDescriptor>(),
                 new ModelStateDictionary()
             );

            _actionContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
            _target = new ONEBookAuthorizeAttribute();
        }

        [Fact]
        public async Task OnAuthorizationAsync_AuthenticatedIsFalse_ForbiddenResult()
        {
            //Arrange
            _mockIdentity.Setup(_ => _.IsAuthenticated).Returns(false);

            var expectedResult = RequestResultConst.ForbiddenResult;

            //Action
            await _target.OnAuthorizationAsync(_actionContext);

            //Assert
            Assert.Equal(expectedResult, _actionContext.Result);
        }

        [Fact]
        public async Task OnAuthorizationAsync_IsAdmin_Bypass()
        {
            //Arrage
            _target.Permissions = FakeHelper.Create<string>();
            _mockIdentity.Setup(_ => _.IsAuthenticated).Returns(true);
            _mockUser.Setup(_ => _.IsInRole("super_admin")).Returns(true);

            var expectedResult = _actionContext.Result;

            //Action
            await _target.OnAuthorizationAsync(_actionContext);

            //Assert
            Assert.Equal(expectedResult, _actionContext.Result);
        }

        [Fact]
        public async Task OnAuthorizationAsync_HasPermission_NoChangeResult()
        {
            //Arrange
            var permission = FakeHelper.Create<string>();
            _target.Permissions = permission;
            _mockIdentity.Setup(_ => _.IsAuthenticated).Returns(true);
            _mockUser.Setup(_ => _.IsInRole(permission)).Returns(true);

            var expectedResult = _actionContext.Result;

            //Action
            await _target.OnAuthorizationAsync(_actionContext);

            //Assert
            Assert.Equal(expectedResult, _actionContext.Result);
        }

        [Fact]
        public async Task OnAuthorizationAsync_HasNotPermission_ForbiddenResult()
        {
            //Arrange
            var permission = FakeHelper.Create<string>();
            _target.Permissions = permission;
            _mockIdentity.Setup(_ => _.IsAuthenticated).Returns(true);
            _mockUser.Setup(_ => _.IsInRole(permission)).Returns(false);

            var expectedResult = RequestResultConst.ForbiddenResult;

            //Action
            await _target.OnAuthorizationAsync(_actionContext);

            //Assert
            Assert.Equal(expectedResult, _actionContext.Result);
        }
    }
}