using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Summary.Controllers;
using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Summary.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Summary.Services;
using Fanex.Athena.AnalyticsAPI.UnitTests.Utils.Helpers;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Summary.UnitTest.Controllers
{
    public class LicenseeWinLossByPlatformController_Test
    {
        private readonly LicenseeWinLossByPlatformController _target;
        private readonly Mock<ILicenseeDataPlatformService> _mockLicenseeDataPlatformService;

        public LicenseeWinLossByPlatformController_Test()
        {
            _mockLicenseeDataPlatformService = new Mock<ILicenseeDataPlatformService>();
            _target = new LicenseeWinLossByPlatformController(Mock.Of<IContextService>(), _mockLicenseeDataPlatformService.Object);
        }

        [Fact]
        public async Task GetData_WithMetaModel_ReturnResultCorrectly()
        {
            //Arrange
            var metaModel = FakeHelper.Create<BaseSummaryMetaModel>();
            var expectedResult = FakeHelper.Create<WidgetByPlatformViewModel>();
            _mockLicenseeDataPlatformService.Setup(_ => _.GetWinLoss(metaModel)).Returns(Task.FromResult(expectedResult));

            //Act
            var result = await _target.GetData(metaModel);
            //Assert
            Assert.Equal(expectedResult, result);

            var now = DateTime.Now.Date;
            Assert.Equal(new DateTime(now.Year, now.Month, 1), metaModel.FromDate);
            Assert.Equal(now, metaModel.ToDate);
        }
    }
}