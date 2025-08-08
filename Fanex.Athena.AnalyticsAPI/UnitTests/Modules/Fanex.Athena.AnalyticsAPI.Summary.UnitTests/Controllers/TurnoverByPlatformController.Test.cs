namespace Fanex.Athena.AnalyticsAPI.Summary.UnitTests.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AnalyticsAPI.UnitTests.Utils.Helpers;
    using Common.Helpers;
    using Common.Services;
    using Core.Services;
    using Models.MetaModels;
    using Models.ViewModels;
    using Moq;
    using Summary.Controllers;
    using Summary.Services;
    using Xunit;

    public class TurnoverByPlatformController_Test
    {
        private readonly TurnoverByPlatformController _target;
        private readonly Mock<ILicenseeDataPlatformService> _mockLicenseeDataPlatformService;
        private readonly Mock<IAuthInfoService> _mockAuthInfoService;

        public TurnoverByPlatformController_Test()
        {
            _mockLicenseeDataPlatformService = new Mock<ILicenseeDataPlatformService>();
            _mockAuthInfoService = new Mock<IAuthInfoService>();
            _target = new TurnoverByPlatformController(Mock.Of<IContextService>(), _mockLicenseeDataPlatformService.Object, _mockAuthInfoService.Object);
        }

        [Fact]
        public async Task GetData_WithMetaModel_ReturnResultCorrectly()
        {
            //Arrange
            var metaModel = FakeHelper.Create<BaseSummaryMetaModel>();
            var expectedResult = FakeHelper.Create<WidgetByPlatformViewModel>();
            _mockLicenseeDataPlatformService.Setup(_ => _.GetTurnover(metaModel)).Returns(Task.FromResult(expectedResult));

            //Act
            var result = await _target.GetData(metaModel);
            //Assert
            Assert.Equal(expectedResult, result);

            var now = DateTimeHelper.DatabaseServerNow.Date;
            Assert.Equal(new DateTime(now.Year, now.Month, 1), metaModel.FromDate);
            Assert.Equal(now, metaModel.ToDate);
        }
    }
}