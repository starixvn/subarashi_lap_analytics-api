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
    public abstract class PlatformPerformanceServiceTestBase
    {
        protected readonly Mock<IContextService> _contextServiceMock;
        protected readonly Mock<IPerformanceRepository> _repositoryMock;
        protected readonly Mock<IPlatformPerformanceVMMapper> _mapperMock;

        protected readonly Fixture _fixture;

        protected readonly PlatformPerformanceService _target;

        protected PlatformPerformanceServiceTestBase()
        {
            _fixture = new Fixture();
            _contextServiceMock = new Mock<IContextService>();
            _repositoryMock = new Mock<IPerformanceRepository>();
            _mapperMock = new Mock<IPlatformPerformanceVMMapper>();

            _target = new PlatformPerformanceService(_contextServiceMock.Object, _repositoryMock.Object, _mapperMock.Object);
        }

        protected static PlatformPerformanceFilterMetaModel FakePlatformPerformanceFilterMetaModel => new PlatformPerformanceFilterMetaModel
        {
            CurrentPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2019", ToMonth = "12/2019" },
            PreviousPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2018", ToMonth = "12/2019" },
            Measurement = new Common.Models.MetaModels.SelectMetaModel(),
            Platforms = Enumerable.Empty<string>()
        };
    }
}