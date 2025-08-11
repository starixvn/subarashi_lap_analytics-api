namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Services
{
    using AutoFixture;
    using Common.Services;
    using Xunit;

    public class CalculationService_CalculateDistribution_Test
    {
        private readonly CalculationService _targetService;
        private readonly Fixture _fixture;

        public CalculationService_CalculateDistribution_Test()
        {
            _targetService = new CalculationService();
            _fixture = new Fixture();
        }

        [Fact]
        public void CalculateDistribution_TotalIsZero_ReturnZero()
        {
            //Arrange
            var value = _fixture.Create<double>();

            //Act
            var actual = _targetService.CalculateDistribution(value, 0);

            //Assert
            Assert.Equal(0, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(null)]
        public void CalculateDistribution_ValueIsNullOrZero_ReturnZero(double? value)
        {
            //Arrange
            //Act
            var actual = _targetService.CalculateDistribution(value, 0);

            //Assert
            Assert.Equal(0, actual);
        }

        [Theory]
        [InlineData(0, 1.25, 0)]
        [InlineData(15, 100, 15)]
        [InlineData(10.55, 98.22, 10.74)]
        public void CalculateDistribution_WhenCall_ReturnCorrectResult(double value, double total, double expected)
        {
            //Arrange

            //Act
            var actual = _targetService.CalculateDistribution(value, total);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}