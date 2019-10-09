using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Moq;
using System;
using System.Collections.Generic;
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

        [Fact]
        public void ToViewModel_PeriodGrowth_EmptyDto_ReturnCorrectResult()
        {
            //Arrange
            _calculationService.Setup(_ => _.ToRound(It.IsAny<double?>(), It.IsAny<int>()))
                .Returns((double? value, int round) =>
                {
                    return value ?? 0;
                });
            _calculationService.Setup(_ => _.ToThousandUnit(It.IsAny<double?>()))
                .Returns((double? value) =>
                {
                    return value ?? 0;
                });
            _calculationService.Setup(_ => _.CalculateGrowth(It.IsAny<double?>(), It.IsAny<double?>()))
                .Returns((double? current, double? previous) =>
                {
                    return !current.HasValue && !previous.HasValue ? 0 : 1;
                });

            _target = new PerformanceVMMapper(_calculationService.Object);

            //Act
            var result = _target.ToViewModel(Enumerable.Empty<TotalDTO>(), Enumerable.Empty<TotalDTO>(), Enumerable.Empty<TotalDTO>());

            //Assert
            Assert.Equal(2, result.Count());
            for (int i = 0; i < 2; i++)
            {
                var _ = result.ElementAt(i);
                Assert.Equal(labels[i], _.Time);
                Assert.Equal(0, _.GrowthPercent);
                Assert.Equal(0, _.Total);
            }
        }

        private static string[] labels = new string[] { "Previous Period", "Current Period" };
    }
}