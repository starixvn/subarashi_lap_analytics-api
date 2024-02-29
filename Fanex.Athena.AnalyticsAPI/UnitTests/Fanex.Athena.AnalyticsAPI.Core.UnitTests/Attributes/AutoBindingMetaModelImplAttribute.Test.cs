using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using Fanex.Athena.AnalyticsAPI.Core.Services;
using Fanex.Athena.AnalyticsAPI.UnitTests.Utils.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Core.UnitTests.Attributes
{
    public class AutoBindingMetaModelImplAttribute_Test
    {
        private AutoBindingMetaModelImplAttribute _target;

        private readonly Mock<IAuthInfoService> _mockAuthInfoService;
        private readonly Mock<IMetaModelBindingService> _mockBindingService;
        private readonly ActionExecutingContext _actionExecutingContext;

        public AutoBindingMetaModelImplAttribute_Test()
        {
            _mockAuthInfoService = new Mock<IAuthInfoService>();
            _mockBindingService = new Mock<IMetaModelBindingService>();

            var actionContext = new ActionContext(
                 Mock.Of<HttpContext>(),
                 Mock.Of<RouteData>(),
                 Mock.Of<ActionDescriptor>(),
                 new ModelStateDictionary()
             );

            _actionExecutingContext = new ActionExecutingContext(
                      actionContext,
                      new List<IFilterMetadata>(),
                      new Dictionary<string, object>(),
                      Mock.Of<Controller>()
                  );
        }

        [Fact]
        public async Task OnActionExecutionAsync_BindSiteName_BindingServiceCallWithParamsCorrectly()
        {
            //Arrange
            var expectedSiteName = FakeHelper.Create<string>();
            _mockAuthInfoService.Setup(_ => _.SiteName).Returns(expectedSiteName);
            _target = new AutoBindingMetaModelImplAttribute(_mockAuthInfoService.Object, _mockBindingService.Object, true, false, false);

            //Action
            await _target.OnActionExecutionAsync(_actionExecutingContext, Mock.Of<ActionExecutionDelegate>());

            //Assert
            _mockBindingService.Verify(_ => _.BindSiteName(_actionExecutingContext.ActionArguments, expectedSiteName), Times.Once);
            _mockBindingService.Verify(_ => _.BindAgentXml(It.IsAny<IDictionary<string, object>>(), It.IsAny<IEnumerable<CurrencyAgentInfo>>()), Times.Never);
        }

        [Fact]
        public async Task OnActionExecutionAsync_BindAgentXml_BindingServiceCallWithParamsCorrectly()
        {
            //Arrange
            var expectedAgentXml = FakeHelper.CreateMany<CurrencyAgentInfo>();
            _mockAuthInfoService.Setup(_ => _.CurrencyAgentList).Returns(expectedAgentXml);
            _target = new AutoBindingMetaModelImplAttribute(_mockAuthInfoService.Object, _mockBindingService.Object, false, true, false);

            //Action
            await _target.OnActionExecutionAsync(_actionExecutingContext, Mock.Of<ActionExecutionDelegate>());

            //Assert
            _mockBindingService.Verify(_ => _.BindAgentXml(_actionExecutingContext.ActionArguments, expectedAgentXml), Times.Once);
            _mockBindingService.Verify(_ => _.BindSiteName(It.IsAny<IDictionary<string, object>>(), It.IsAny<string>()), Times.Never);
        }
    }
}