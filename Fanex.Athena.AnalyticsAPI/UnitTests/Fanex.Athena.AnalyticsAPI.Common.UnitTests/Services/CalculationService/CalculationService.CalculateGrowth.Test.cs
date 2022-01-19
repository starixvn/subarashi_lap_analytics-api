using Fanex.Athena.AnalyticsAPI.Common.Services;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Services
{
    public class CalculationService_CalculateGrowth_Test
    {
        private readonly CalculationService _targetService;

        public CalculationService_CalculateGrowth_Test()
        {
            _targetService = new CalculationService();
        }

        [Theory]
        [InlineData(null, 0, 0)]
        [InlineData(0, null, 0)]
        [InlineData(null, null, 0)]
        [InlineData(-123, null, -100)]
        [InlineData(1235, 0, 100)]
        public void CalculateGrowth_PreviousIsNullOrZero_ReturnCorrectResult(double? current, double? previous, double expected)
        {
            //Arrange
            //Act
            var actual = _targetService.CalculateGrowth(current, previous);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, -123.123, 100)]
        [InlineData(0, 123.123, -100)]
        [InlineData(null, -123.123, 100)]
        [InlineData(null, 123.123, -100)]
        public void CalculateGrowth_CurrentIsNullOrZero_ReturnCorrectResult(double? current, double? previous, double expected)
        {
            //Arrange
            //Act
            var actual = _targetService.CalculateGrowth(current, previous);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1.23, 1.23, 0)]
        [InlineData(1, 100, -99)]
        [InlineData(1.5, 100.23, -98.5)]
        [InlineData(-50, -100, 50)]
        [InlineData(100, 50, 100)]
        public void CalculateGrowth_NotNullAndNotZero_ReturnCorrectResult(double? current, double? previous, double expected)
        {
            //Arrange

            //Act
            var actual = _targetService.CalculateGrowth(current, previous);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}