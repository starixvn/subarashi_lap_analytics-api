using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models.Criteria;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.UnitTests.Models.Criteria
{
    public class SportsbookCriteria_Test
    {
        private readonly Fixture _fixture;

        public SportsbookCriteria_Test()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void GetSettingKey_Return_RightSettingKey()
        {
            // Arrange
            var criteria = _fixture.Create<SportsbookCriteria>();

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("LAP_Sportbook_Performance_MeasureDistributionBySite", result);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnTrue()
        {
            // Arrange
            var criteria = _fixture.Create<SportsbookCriteria>();
            criteria.SiteName = "";
            criteria.Type = Constants.TabType.SPORT;
            criteria.SportType = Constants.SportType.ALL;

            // Assert
            Assert.True(criteria.IsValid());
        }
    }
}