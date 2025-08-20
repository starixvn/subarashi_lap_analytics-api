namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Services
{
    using System.Linq;
    using AutoFixture;
    using Common.Models.MetaModels;
    using Common.Services;
    using Moq;
    using Performance.Mappers;
    using Performance.Models.MetaModels;
    using Performance.Services;
    using Repositories;

    public abstract class PlatformPerformanceServiceTestBase
    {
        protected readonly Mock<IContextService> _contextServiceMock;
        protected readonly Mock<IPerformanceRepository> _repositoryMock;
        protected readonly Mock<IPlatformPerformanceVMMapper> _mapperMock;
        protected readonly Mock<IHandleRawData> _handleRawDataMock;

        protected readonly Fixture _fixture;

        protected readonly PlatformPerformanceService _target;

        protected PlatformPerformanceServiceTestBase()
        {
            _fixture = new Fixture();
            _contextServiceMock = new Mock<IContextService>();
            _repositoryMock = new Mock<IPerformanceRepository>();
            _mapperMock = new Mock<IPlatformPerformanceVMMapper>();
            _handleRawDataMock = new Mock<IHandleRawData>();

            _target = new PlatformPerformanceService(_contextServiceMock.Object, _repositoryMock.Object, _mapperMock.Object, _handleRawDataMock.Object);
        }

        protected static PlatformPerformanceFilterMetaModel FakePlatformPerformanceFilterMetaModel => new PlatformPerformanceFilterMetaModel
        {
            CurrentPeriod = new MonthRangeMetaModel { FromMonth = "01/2019", ToMonth = "12/2019" },
            PreviousPeriod = new MonthRangeMetaModel { FromMonth = "01/2018", ToMonth = "12/2019" },
            Measurement = new SelectMetaModel(),
            Platforms = Enumerable.Empty<string>(),
            TopDistributions = Enumerable.Empty<DistributionMetaModel>()
        };
    }
}