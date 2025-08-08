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

    public abstract class BettypePerformanceServiceTestBase
    {
        protected readonly Mock<IContextService> _contextServiceMock;
        protected readonly Mock<IPerformanceRepository> _repositoryMock;
        protected readonly Mock<IBettypePerformanceVMMapper> _mapperMock;
        protected readonly Mock<IHandleRawData> _handleRawDataMock;

        protected readonly Fixture _fixture;

        protected readonly BettypePerformanceService _target;

        protected BettypePerformanceServiceTestBase()
        {
            _fixture = new Fixture();
            _contextServiceMock = new Mock<IContextService>();
            _repositoryMock = new Mock<IPerformanceRepository>();
            _mapperMock = new Mock<IBettypePerformanceVMMapper>();
            _handleRawDataMock = new Mock<IHandleRawData>();

            _target = new BettypePerformanceService(_contextServiceMock.Object, _repositoryMock.Object, _mapperMock.Object, _handleRawDataMock.Object);
        }

        protected static BettypePerformanceFilterMetaModel FakBettypePerformanceFilterMetaModel => new BettypePerformanceFilterMetaModel
        {
            CurrentPeriod = new MonthRangeMetaModel { FromMonth = "01/2019", ToMonth = "12/2019" },
            PreviousPeriod = new MonthRangeMetaModel { FromMonth = "01/2018", ToMonth = "12/2019" },
            Measurement = new SelectMetaModel(),
            CombineIds = Enumerable.Empty<string>(),
            TopDistributions = Enumerable.Empty<DistributionMetaModel>()
        };
    }
}