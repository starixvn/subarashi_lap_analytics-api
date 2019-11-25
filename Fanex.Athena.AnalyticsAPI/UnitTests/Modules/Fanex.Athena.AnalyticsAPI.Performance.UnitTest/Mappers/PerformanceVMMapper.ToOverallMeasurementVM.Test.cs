using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Moq;
using System.Linq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Mappers
{
    public class PerformanceVMMapper_ToOverallMeasurementVM_Test
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICalculationService> _calculationService;
        private readonly PerformanceVMMapper _target;

        public PerformanceVMMapper_ToOverallMeasurementVM_Test()
        {
            _fixture = new Fixture();
            _calculationService = new Mock<ICalculationService>();
            _target = new PerformanceVMMapper(_calculationService.Object);

            _calculationService.Setup(_ => _.ToRound(It.IsAny<double?>(), 2))
                .Returns((double? value, int numberOfRound) => value ?? 0);
        }

        [Fact]
        public void ToOverallMeasurement_AnyInput_ReturnCorrectResult()
        {
            //Arrange

            //Act
            var result = _target.ToOverallMeasurementVM(It.IsAny<TotalDTO>(), It.IsAny<TotalDTO>());

            //Assert
            Assert.Equal(2, result.Count());

            Assert.Equal("Previous Period", result.ElementAt(0)?.Name);
            Assert.Equal("Current Period", result.ElementAt(1)?.Name);
        }

        [Fact]
        public void ToOverallMeasurement_InputIsNull_ReturnZeroAsTotal()
        {
            //Arrange

            //Act
            var result = _target.ToOverallMeasurementVM(null as TotalDTO, null as TotalDTO);

            //Assert
            Assert.True(result.All(_ => _.Total == 0));
        }

        [Theory]
        [InlineData(500, 100)]
        [InlineData(-999.678, 2239923.12)]
        [InlineData(-500.123, 100.1235)]
        public void ToOverallMeasurement_WhenCall_ReturnCorrectOfTotal(double prevPeriodTotal, double currentPeriodTotal)
        {
            //Arrange
            var prevPeriodDto = new TotalDTO() { Total = prevPeriodTotal };
            var currentPeriodDto = new TotalDTO() { Total = currentPeriodTotal };

            //Act
            var result = _target.ToOverallMeasurementVM(prevPeriodDto, currentPeriodDto);

            //Assert
            Assert.Equal(prevPeriodDto.Total, result.ElementAt(0).Total);
            Assert.Equal(currentPeriodDto.Total, result.ElementAt(1).Total);
        }
    }
}