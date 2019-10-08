using Fanex.Athena.AnalyticsAPI.Common.Services;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTest.Services
{
    public class CalculationService_ToThousandUnit_Test
    {
        private readonly CalculationService _targetService;

        public CalculationService_ToThousandUnit_Test()
        {
            _targetService = new CalculationService();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(null, 0)]
        [InlineData(1123.246, 1.12)]
        [InlineData(1147.246, 1.15)]
        [InlineData(500, 0.5)]
        [InlineData(1, 0.00)]
        public void ToThousandUnit_WhenCall_ReturnCorrectResult(double? value, double expected)
        {
            //Arrange
            //Act
            var actual = _targetService.ToThousandUnit(value);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}