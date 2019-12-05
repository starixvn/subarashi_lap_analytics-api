using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Moq;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Services
{
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
            CurrentPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2019", ToMonth = "12/2019" },
            PreviousPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2018", ToMonth = "12/2019" },
            Measurement = new Common.Models.MetaModels.SelectMetaModel(),
            Products = Enumerable.Empty<string>()
        };
    }
}