using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Summary.Controllers;
using Fanex.Athena.AnalyticsAPI.Summary.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Summary.Services;
using Fanex.Athena.AnalyticsAPI.UnitTests.Utils.Helpers;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Summary.UnitTests.Controllers
{
    public class CustomerBalanceController_Test
    {
        private readonly CustomerBalanceController _target;
        private readonly Mock<ISingleValueService> _mockSingleValueService;

        public CustomerBalanceController_Test()
        {
            _mockSingleValueService = new Mock<ISingleValueService>();
            _target = new CustomerBalanceController(Mock.Of<IContextService>(), _mockSingleValueService.Object);
        }

        [Fact]
        public async Task GetData_WithMetaModel_ReturnResultCorrectly()
        {
            //Arrange
            var metaModel = FakeHelper.Create<BaseSummaryMetaModel>();
            var expectedResult = FakeHelper.Create<decimal>();
            _mockSingleValueService.Setup(_ => _.GetCustomerBalance(metaModel)).Returns(Task.FromResult(expectedResult));

            //Act
            var result = await _target.GetData(metaModel);

            //Assert
            Assert.Equal(expectedResult, result);
            Assert.Equal(DateTimeHelper.DatabaseServerNow.Date, metaModel.FromDate);
        }
    }
}