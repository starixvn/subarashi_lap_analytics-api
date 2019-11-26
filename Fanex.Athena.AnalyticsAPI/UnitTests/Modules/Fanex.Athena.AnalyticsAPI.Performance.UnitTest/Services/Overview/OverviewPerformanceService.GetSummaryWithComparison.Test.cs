using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Data.Repository;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Services
{
    public class OverviewPerformanceService_GetSummaryWithComparison_Test : OverviewPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetSummaryWithComparison_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;
            var data = new List<IEnumerable<SummaryDTO>> { _fixture.CreateMany<SummaryDTO>(), _fixture.CreateMany<SummaryDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<SummaryDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .ReturnsAsync(data);

            //Act

            await _target.GetSummaryWithComparison(filter);

            //Assert
            _mapperMock.Verify(_ => _.ToSummaryVM(data[0], data[1]), Times.Once);
        }

        [Fact]
        public async Task GetSummaryWithComparison_WhenCall_ServiceResultIsMapperResult()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;
            var expected = _fixture.CreateMany<SummaryVM>();

            _mapperMock.Setup(_ => _.ToSummaryVM(It.IsAny<IEnumerable<SummaryDTO>>(), It.IsAny<IEnumerable<SummaryDTO>>()))
                .Returns(expected);

            //Act

            var result = await _target.GetSummaryWithComparison(filter);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}