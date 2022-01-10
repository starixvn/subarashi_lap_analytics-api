using Fanex.Athena.AnalyticsAPI.Common.Services;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Services
{
    public class CalculationService_ToRound_Test
    {
        private readonly CalculationService _targetService;

        public CalculationService_ToRound_Test()
        {
            _targetService = new CalculationService();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(null, 0)]
        [InlineData(1.246, 1.25)]
        [InlineData(1.256, 1.26)]
        [InlineData(1.266, 1.27)]
        public void ToRound_WhenCall_ReturnCorrectResult(double? value, double expected)
        {
            //Arrange
            //Act
            var actual = _targetService.ToRound(value);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}