namespace Fanex.Athena.AnalyticsAPI.Summary.UnitTests.Controllers
{
    using System.Threading.Tasks;
    using AnalyticsAPI.UnitTests.Utils.Helpers;
    using Common.Helpers;
    using Common.Services;
    using Models.MetaModels;
    using Moq;
    using Summary.Controllers;
    using Summary.Services;
    using Xunit;

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