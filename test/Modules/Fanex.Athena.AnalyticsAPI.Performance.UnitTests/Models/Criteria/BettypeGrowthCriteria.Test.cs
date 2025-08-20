namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Models.Criteria
{
    using AutoFixture;
    using Performance.Models.Criteria;
    using Xunit;

    public class BettypeGrowthCriteria_Test
    {
        private readonly Fixture _fixture;

        public BettypeGrowthCriteria_Test()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void GetSettingKey_Return_RightSettingKey()
        {
            // Arrange
            var criteria = _fixture.Create<BettypeGrowthCriteria>();

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("LAP_Performance_BettypeComparionBySite", result);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnTrue()
        {
            // Arrange
            var criteria = _fixture.Create<BettypeGrowthCriteria>();
            criteria.SiteName = "";

            // Assert
            Assert.True(criteria.IsValid());
        }
    }
}