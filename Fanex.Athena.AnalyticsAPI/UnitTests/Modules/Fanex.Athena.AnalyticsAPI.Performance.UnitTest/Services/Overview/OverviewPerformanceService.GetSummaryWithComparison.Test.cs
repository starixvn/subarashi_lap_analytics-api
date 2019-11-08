using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Moq;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Services
{
    public class OverviewPerformanceService_GetSummaryWithComparison_Test
    {
        private readonly OverviewPerformanceService _targetService;
        private readonly Mock<IContextService> _contextMock;
        private readonly Mock<IPerformanceRepository> _repoMock;
        private readonly Mock<IOverviewPerformanceVMMapper> _viewModelMapperMock;
        private readonly Fixture _fixture;

        public OverviewPerformanceService_GetSummaryWithComparison_Test()
        {
            _contextMock = new Mock<IContextService>();
            _repoMock = new Mock<IPerformanceRepository>();
            _viewModelMapperMock = new Mock<IOverviewPerformanceVMMapper>();
            _targetService = new OverviewPerformanceService(_contextMock.Object, _repoMock.Object, _viewModelMapperMock.Object);

            _fixture = new Fixture();
        }

        public void GetSummaryWithComparison_WhenCall_ReturnCorrectResult()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}