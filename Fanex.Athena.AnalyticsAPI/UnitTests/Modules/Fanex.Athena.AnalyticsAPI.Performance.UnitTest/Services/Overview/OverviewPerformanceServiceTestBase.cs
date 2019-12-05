using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Moq;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Services
{
    public abstract class OverviewPerformanceServiceTestBase
    {
        protected readonly Mock<IContextService> _contextServiceMock;
        protected readonly Mock<IPerformanceRepository> _repositoryMock;
        protected readonly Mock<IOverviewPerformanceVMMapper> _mapperMock;
        protected readonly Mock<IHandleRawData> _handleRawDataMock;

        protected readonly Fixture _fixture;

        protected readonly OverviewPerformanceService _target;

        protected OverviewPerformanceServiceTestBase()
        {
            _fixture = new Fixture();
            _contextServiceMock = new Mock<IContextService>();
            _repositoryMock = new Mock<IPerformanceRepository>();
            _mapperMock = new Mock<IOverviewPerformanceVMMapper>();
            _handleRawDataMock = new Mock<IHandleRawData>();

            _target = new OverviewPerformanceService(_contextServiceMock.Object, _repositoryMock.Object, _mapperMock.Object, _handleRawDataMock.Object);
        }

        protected static PerformanceFilterMetaModel FakePerformanceFilterMetaModel => new PerformanceFilterMetaModel
        {
            CurrentPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2019", ToMonth = "12/2019" },
            PreviousPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2018", ToMonth = "12/2019" },
            Measurement = new Common.Models.MetaModels.SelectMetaModel()
        };
    }
}