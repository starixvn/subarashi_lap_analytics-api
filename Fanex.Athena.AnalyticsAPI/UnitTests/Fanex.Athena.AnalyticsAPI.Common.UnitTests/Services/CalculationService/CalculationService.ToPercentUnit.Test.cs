using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Services
{
    public class CalculationService_ToPercentUnit_Test
    {
        private readonly CalculationService _targetService;
        private readonly Fixture _fixture;

        public CalculationService_ToPercentUnit_Test()
        {
            _targetService = new CalculationService();
            _fixture = new Fixture();
        }

        [Fact]
        public void ToPercentUnit_ValueIsNull_ReturnZero()
        {
            //Arrange

            //Act
            var actual = _targetService.ToPercentUnit(null);

            //Assert
            Assert.Equal(0, actual);
        }


        [Theory]
        [InlineData(0.86732, 86.73)]
        [InlineData(0.001, 0.1)]
        [InlineData(0.91235, 91.24)]
        public void ToPercentUnit_WhenCall_ReturnCorrectResult(double value, double expected)
        {
            //Arrange

            //Act
            var actual = _targetService.ToPercentUnit(value);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}