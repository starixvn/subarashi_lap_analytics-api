namespace Fanex.Athena.AnalyticsAPI.Core.UnitTests.Helpers
{
    using AutoFixture;
    using Core.Helpers;
    using Models.DataDomain;
    using Xunit;

    public class SabaHelperTests
    {
        private readonly Fixture _fixture;

        public SabaHelperTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void IsHideSaba_WithJollysports_ReturnsTrue()
        {
            // Arrange
            var specialPlatform = SpecialPlatform.Jollysports;

            // Act
            var result = SabaHelper.IsHideSaba(specialPlatform);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsHideSaba_WithJollysportCV_ReturnsTrue()
        {
            // Arrange
            var specialPlatform = SpecialPlatform.JollysportCV;

            // Act
            var result = SabaHelper.IsHideSaba(specialPlatform);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsHideSaba_WithOtherPlatform_ReturnsFalse()
        {
            // Arrange
            var specialPlatform = 0; // Assuming 0 is not Jollysports or JollysportCV

            // Act
            var result = SabaHelper.IsHideSaba(specialPlatform);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsHideSaba_WithNegativeValue_ReturnsFalse()
        {
            // Arrange
            var specialPlatform = -1;

            // Act
            var result = SabaHelper.IsHideSaba(specialPlatform);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(100)]
        [InlineData(999)]
        public void IsHideSaba_WithVariousNonJollyPlatforms_ReturnsFalse(int specialPlatform)
        {
            // Act
            var result = SabaHelper.IsHideSaba(specialPlatform);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsHideSaba_WithRandomValue_ReturnsFalse()
        {
            // Arrange
            var randomPlatform = _fixture.Create<int>();
            // Ensure it's not Jollysports or JollysportCV
            while (randomPlatform == SpecialPlatform.Jollysports || randomPlatform == SpecialPlatform.JollysportCV)
            {
                randomPlatform = _fixture.Create<int>();
            }

            // Act
            var result = SabaHelper.IsHideSaba(randomPlatform);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsHideSaba_WithMaxValue_ReturnsFalse()
        {
            // Arrange
            var specialPlatform = int.MaxValue;

            // Act
            var result = SabaHelper.IsHideSaba(specialPlatform);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsHideSaba_WithMinValue_ReturnsFalse()
        {
            // Arrange
            var specialPlatform = int.MinValue;

            // Act
            var result = SabaHelper.IsHideSaba(specialPlatform);

            // Assert
            Assert.False(result);
        }
    }
}
