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

    public abstract class ProductPerformanceServiceTestBase
    {
        protected readonly Mock<IContextService> _contextServiceMock;
        protected readonly Mock<IPerformanceRepository> _repositoryMock;
        protected readonly Mock<IProductPerformanceVMMapper> _mapperMock;
        protected readonly Mock<IHandleRawData> _handleRawDataMock;

        protected readonly Fixture _fixture;

        protected readonly ProductPerformanceService _target;

        protected ProductPerformanceServiceTestBase()
        {
            _fixture = new Fixture();
            _contextServiceMock = new Mock<IContextService>();
            _repositoryMock = new Mock<IPerformanceRepository>();
            _mapperMock = new Mock<IProductPerformanceVMMapper>();
            _handleRawDataMock = new Mock<IHandleRawData>();

            _target = new ProductPerformanceService(_contextServiceMock.Object, _repositoryMock.Object, _mapperMock.Object, _handleRawDataMock.Object);
        }

        protected static ProductPerformanceFilterMetaModel FakeProductPerformanceFilterMetaModel => new ProductPerformanceFilterMetaModel
        {
            CurrentPeriod = new MonthRangeMetaModel { FromMonth = "01/2019", ToMonth = "12/2019" },
            PreviousPeriod = new MonthRangeMetaModel { FromMonth = "01/2018", ToMonth = "12/2019" },
            Measurement = new SelectMetaModel(),
            Products = Enumerable.Empty<string>(),
            TopDistributions = Enumerable.Empty<DistributionMetaModel>()
        };
    }
}