namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Models.Criteria
{
    using AutoFixture;
    using Performance.Models.Criteria;
    using Xunit;

    public class CurrencyDistributionCriteria_Test
    {
        private readonly Fixture _fixture;

        public CurrencyDistributionCriteria_Test()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void GetSettingKey_Return_RightSettingKey()
        {
            // Arrange
            var criteria = _fixture.Create<CurrencyDistributionCriteria>();

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("LAP_Performance_CurrencyMeasureDistributionBySite", result);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnTrue()
        {
            // Arrange
            var criteria = _fixture.Create<CurrencyGrowthCriteria>();
            criteria.SiteName = "";

            // Assert
            Assert.True(criteria.IsValid());
        }
    }
}