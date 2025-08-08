namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Models.Criteria
{
    using AutoFixture;
    using Performance.Models.Criteria;
    using Xunit;

    public class PlatformDistributionCriteria_Test
    {
        private readonly Fixture _fixture;

        public PlatformDistributionCriteria_Test()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void GetSettingKey_Return_RightSettingKey()
        {
            // Arrange
            var criteria = _fixture.Create<PlatformDistributionCriteria>();

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("LAP_Performance_PlatformMeasureDistributionBySite", result);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnTrue()
        {
            // Arrange
            var criteria = _fixture.Create<PlatformDistributionCriteria>();
            criteria.SiteName = "";

            // Assert
            Assert.True(criteria.IsValid());
        }
    }
}