using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Mappers
{
    public class OverviewPerformanceVMMapper_ToMeasurementGrowthPeriodVM_Test
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICalculationService> _calculationService;
        private readonly OverviewPerformanceVMMapper _target;

        public OverviewPerformanceVMMapper_ToMeasurementGrowthPeriodVM_Test()
        {
            _fixture = new Fixture();
            _calculationService = new Mock<ICalculationService>();
            _target = new OverviewPerformanceVMMapper(_calculationService.Object);

            _calculationService.Setup(_ => _.ToRound(It.IsAny<double?>(), 2))
                .Returns((double? value, int numberOfRound) => value ?? 0);

            _calculationService.Setup(_ => _.CalculateGrowth(It.IsAny<double?>(), It.IsAny<double?>()))
              .Returns((double? current, double? previous) => (current ?? 0) + (previous ?? 0));
        }

        [Theory]
        [InlineData(92132, 123, 123)]
        [InlineData(5132, 1235.123, -1883)]
        [InlineData(92132.888, 1235.123, -1583)]
        public void ToMeasurementGrowthPeriodVM_WhenCall_ReturnCorrectResult(double total0, double total1, double total2)
        {
            //Arrange
            var prevPeriodOfPrevDtos = new List<TotalDTO> { new TotalDTO { Total = total0 } };
            var prevPeriodData = new List<TotalDTO> { new TotalDTO { Total = total1 } };
            var currentPeriodData = new List<TotalDTO> { new TotalDTO { Total = total2 } };

            var expected = new List<MeasurementGrowthVM>
            {
                new MeasurementGrowthVM
                {
                    Id = "previous_period",
                    Name = "Previous Period",
                    Total = total1,
                    GrowthPercent =  total1 + total0
                },
                new MeasurementGrowthVM
                {
                    Id = "current_period",
                    Name = "Current Period",
                    Total = total2,
                    GrowthPercent =  total2 + total1
                }
            };

            //Act
            var result = _target.ToMeasurementGrowthPeriodVM(prevPeriodOfPrevDtos, prevPeriodData, currentPeriodData);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }
    }
}