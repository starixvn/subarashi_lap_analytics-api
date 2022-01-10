using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Data;
using Fanex.Data.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Services
{
    public class DaxQueryContentService_GetDaxQuery_Test
    {
        private readonly DaxQueryContentService _targetService;
        private readonly Fixture _fixture;
        private readonly Mock<ISqlService> _sqlService;

        public DaxQueryContentService_GetDaxQuery_Test()
        {
            _sqlService = new Mock<ISqlService>();
            _targetService = new DaxQueryContentService(_sqlService.Object);

            _fixture = new Fixture();

            DbSettingProviderManager.StartTestingSession();
            ObjectDbFactory.SwitchToTestingMode();
        }

        [Fact]
        public async Task GetDaxQuery_OutputNull_ReturnEmpty()
        {
            //Arrange
            var criteria = new Mock<ICriteria>().Object;
            _sqlService.Setup(_ => _.ExecuteNonQueryWithOutput(criteria, "DAX_Query")).Returns(Task.FromResult<object>(null));

            //Act
            var result = await _targetService.GetDaxQuery(criteria);

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetDaxQuery_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var criteria = new Mock<ICriteria>().Object;
            var output = "output result";

            _sqlService.Setup(_ => _.ExecuteNonQueryWithOutput(criteria, "DAX_Query")).Returns(Task.FromResult<object>(output));

            //Act
            var result = await _targetService.GetDaxQuery(criteria);

            //Assert
            Assert.Equal(output.ToString(), result.ToString());
        }
    }
}