namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Models.Criteria
{
    using AutoFixture;
    using Performance.Models.Criteria;
    using Xunit;

    public class OverviewTopChangeCriteria_Test
    {
        private readonly Fixture _fixture;

        public OverviewTopChangeCriteria_Test()
        {
            _fixture = new Fixture();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetSettingKey_Return_RightSettingKey(byte dimesion)
        {
            // Arrange
            var settingKeys = new[]
            {
                "LAP_Performance_OverviewCurencyBySite",
                "LAP_Performance_OverviewBetypeBySite",
                "LAP_Performance_OverviewPlatFormBySite",
                "LAP_Performance_OverviewProductBySite"
            };
            var criteria = _fixture.Create<OverviewTopChangeCriteria>();
            criteria.Dimension = dimesion;

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal(settingKeys[dimesion], result);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnTrue()
        {
            // Arrange
            var criteria = _fixture.Create<OverviewTopChangeCriteria>();
            criteria.SiteName = "";

            // Assert
            Assert.True(criteria.IsValid());
        }
    }
}