namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Helpers
{
    using AutoFixture;
    using Common.Helpers;
    using Xunit;

    public class CurrencyHelperTests
    {
        private readonly Fixture _fixture;

        public CurrencyHelperTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void GetBasedCurrency_WithMP_ReturnsPhp()
        {
            // Arrange
            var currency = "MP";

            // Act
            var result = CurrencyHelper.GetBasedCurrency(currency);

            // Assert
            Assert.Equal("PHP", result);
        }

        [Fact]
        public void GetBasedCurrency_WithMpLowercase_ReturnsPhp()
        {
            // Arrange
            var currency = "mp";

            // Act
            var result = CurrencyHelper.GetBasedCurrency(currency);

            // Assert
            Assert.Equal("PHP", result);
        }

        [Fact]
        public void GetBasedCurrency_WithMpMixedCase_ReturnsPhp()
        {
            // Arrange
            var currency = "Mp";

            // Act
            var result = CurrencyHelper.GetBasedCurrency(currency);

            // Assert
            Assert.Equal("PHP", result);
        }

        [Fact]
        public void GetBasedCurrency_WithMpUppercase_ReturnsPhp()
        {
            // Arrange
            var currency = "MP";

            // Act
            var result = CurrencyHelper.GetBasedCurrency(currency);

            // Assert
            Assert.Equal("PHP", result);
        }

        [Theory]
        [InlineData("USD")]
        [InlineData("EUR")]
        [InlineData("GBP")]
        [InlineData("CNY")]
        [InlineData("JPY")]
        public void GetBasedCurrency_WithOtherCurrencies_ReturnsSameCurrency(string currency)
        {
            // Act
            var result = CurrencyHelper.GetBasedCurrency(currency);

            // Assert
            Assert.Equal(currency, result);
        }

        [Fact]
        public void GetBasedCurrency_WithNull_ReturnsNull()
        {
            // Act
            var result = CurrencyHelper.GetBasedCurrency(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetBasedCurrency_WithEmptyString_ReturnsEmptyString()
        {
            // Act
            var result = CurrencyHelper.GetBasedCurrency(string.Empty);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetBasedCurrency_WithWhitespace_ReturnsWhitespace()
        {
            // Arrange
            var currency = "   ";

            // Act
            var result = CurrencyHelper.GetBasedCurrency(currency);

            // Assert
            Assert.Equal(currency, result);
        }

        [Theory]
        [InlineData("PHP")]
        [InlineData("THB")]
        [InlineData("IDR")]
        [InlineData("MYR")]
        public void GetBasedCurrency_WithNonMpCurrencies_ReturnsOriginal(string currency)
        {
            // Act
            var result = CurrencyHelper.GetBasedCurrency(currency);

            // Assert
            Assert.Equal(currency, result);
        }

        [Fact]
        public void GetBasedCurrency_WithRandomString_ReturnsOriginal()
        {
            // Arrange
            var currency = _fixture.Create<string>();

            // Act
            var result = CurrencyHelper.GetBasedCurrency(currency);

            // Assert
            Assert.Equal(currency, result);
        }

        [Theory]
        [InlineData("MPX")]
        [InlineData("XMP")]
        [InlineData("MPTEST")]
        public void GetBasedCurrency_WithStringContainingMp_ReturnsOriginal(string currency)
        {
            // Act
            var result = CurrencyHelper.GetBasedCurrency(currency);

            // Assert
            Assert.Equal(currency, result);
        }

        [Theory]
        [InlineData("M P")]
        [InlineData("M-P")]
        [InlineData("M_P")]
        public void GetBasedCurrency_WithMpVariations_ReturnsOriginal(string currency)
        {
            // Act
            var result = CurrencyHelper.GetBasedCurrency(currency);

            // Assert
            Assert.Equal(currency, result);
        }
    }
}
