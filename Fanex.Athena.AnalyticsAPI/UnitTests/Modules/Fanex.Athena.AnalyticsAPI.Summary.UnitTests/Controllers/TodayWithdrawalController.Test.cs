using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Summary.Controllers;
using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Summary.Services;
using Fanex.Athena.AnalyticsAPI.UnitTests.Utils.Helpers;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Summary.UnitTests.Controllers
{
    public class TodayWithdrawalController_Test
    {
        private readonly TodayWithdrawalController _target;
        private readonly Mock<ISingleValueService> _mockSingleValueService;

        public TodayWithdrawalController_Test()
        {
            _mockSingleValueService = new Mock<ISingleValueService>();
            _target = new TodayWithdrawalController(Mock.Of<IContextService>(), _mockSingleValueService.Object);
        }

        [Fact]
        public async Task GetData_WithMetaModel_ReturnResultCorrectly()
        {
            //Arrange
            var metaModel = FakeHelper.Create<BaseSummaryMetaModel>();
            var expectedResult = FakeHelper.Create<decimal>();
            _mockSingleValueService.Setup(_ => _.GetTodayWithdrawal(metaModel)).Returns(Task.FromResult(expectedResult));

            //Act
            var result = await _target.GetData(metaModel);

            //Assert
            Assert.Equal(expectedResult, result);
            Assert.Equal(DateTimeHelper.DatabaseServerNow.Date, metaModel.FromDate);
        }
    }
}