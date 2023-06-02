using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Models.Criteria
{
    public class PlatformMeasurementComparisonCriteria_Test
    {
        private readonly Fixture _fixture;

        public PlatformMeasurementComparisonCriteria_Test()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void GetSettingKey_Return_RightSettingKey()
        {
            // Arrange
            var criteria = _fixture.Create<PlatformMeasurementComparisonCriteria>();

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("LAP_Performance_PlatformComparionPeriodBySite", result);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnTrue()
        {
            // Arrange
            var criteria = _fixture.Create<PlatformMeasurementComparisonCriteria>();
            criteria.SiteName = "";

            // Assert
            Assert.True(criteria.IsValid());
        }
    }
}