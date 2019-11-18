using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Moq;
using System.Linq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Mappers
{
    public class PerformanceVMMapper_Test
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICalculationService> _calculationService;

        private PerformanceVMMapper _target;

        public PerformanceVMMapper_Test()
        {
            _fixture = new Fixture();
            _calculationService = new Mock<ICalculationService>();
        }

        private static string[] labels = new string[] { "Previous Period", "Current Period" };
    }
}