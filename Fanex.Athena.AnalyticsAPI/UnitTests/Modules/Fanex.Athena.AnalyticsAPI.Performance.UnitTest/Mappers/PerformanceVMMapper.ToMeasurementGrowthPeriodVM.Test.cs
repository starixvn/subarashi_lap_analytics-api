using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Moq;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Mappers
{
    public class PerformanceVMMapper_ToMeasurementGrowthPeriodVM_Test
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICalculationService> _calculationService;
        private readonly PerformanceVMMapper _target;

        public PerformanceVMMapper_ToMeasurementGrowthPeriodVM_Test()
        {
            _fixture = new Fixture();
            _calculationService = new Mock<ICalculationService>();
            _target = new PerformanceVMMapper(_calculationService.Object);
        }

        public static IEnumerable<(string topDistributionId, string topDistributionName)> FakeTopDistributions()
        {
            return new List<(string topDistributionId, string topDistributionName)>
            {
                ("1", "distribution1"),
                ("2", "distribution2"),
                ("3", "distribution3"),
                ("4", "distribution4"),
                ("5", "distribution5"),
                ("0", "Others")
            };
        }
    }
}