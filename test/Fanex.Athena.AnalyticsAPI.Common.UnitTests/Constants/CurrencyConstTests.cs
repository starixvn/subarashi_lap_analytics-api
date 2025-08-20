namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Constants
{
    using System.Collections.Generic;
    using System.Linq;
    using Consts;
    using Xunit;

    public class CurrencyConstTests
    {
        [Fact]
        public void SupportedBaseCurrencies_ContainsExpectedCurrencies()
        {
            // Act
            var currencies = CurrencyConst.SupportedBaseCurrencies;

            // Assert
            Assert.NotNull(currencies);
            Assert.NotEmpty(currencies);

            // Verify specific currencies from the actual implementation
            Assert.Contains("USD", currencies);
            Assert.Contains("EUR", currencies);
            Assert.Contains("GBP", currencies);
            Assert.Contains("CNY", currencies);
            Assert.Contains("JPY", currencies);
            Assert.Contains("MYR", currencies);
            Assert.Contains("THB", currencies);
            Assert.Contains("IDR", currencies);
            Assert.Contains("USDT", currencies);
        }

        [Fact]
        public void SupportedBaseCurrencies_IsReadOnly()
        {
            // Act
            var currencies = CurrencyConst.SupportedBaseCurrencies;

            // Assert
            Assert.IsAssignableFrom<IReadOnlyCollection<string>>(currencies);
        }

        [Fact]
        public void SupportedBaseCurrencies_ContainsOnlyUppercaseCurrencies()
        {
            // Act
            var currencies = CurrencyConst.SupportedBaseCurrencies;

            // Assert
            foreach (var currency in currencies)
            {
                Assert.Equal(currency.ToUpperInvariant(), currency);
                // Most currency codes are 3 characters, but some like USDT are 4
                Assert.True(currency.Length >= 2 && currency.Length <= 4,
                    $"Currency {currency} should be 2-4 characters long");
            }
        }

        [Theory]
        [InlineData("USD")]
        [InlineData("EUR")]
        [InlineData("GBP")]
        [InlineData("CNY")]
        [InlineData("JPY")]
        [InlineData("MYR")]
        [InlineData("THB")]
        [InlineData("IDR")]
        [InlineData("USDT")]
        [InlineData("MP")]
        [InlineData("BRL")]
        [InlineData("ZAR")]
        public void SupportedBaseCurrencies_ContainsSpecificCurrency(string currency)
        {
            // Act
            var currencies = CurrencyConst.SupportedBaseCurrencies;

            // Assert
            Assert.Contains(currency, currencies);
        }

        [Theory]
        [InlineData("INVALID")]
        [InlineData("XXX")]
        [InlineData("")]
        [InlineData("US")]
        [InlineData("USDD")]
        public void SupportedBaseCurrencies_DoesNotContainInvalidCurrency(string currency)
        {
            // Act
            var currencies = CurrencyConst.SupportedBaseCurrencies;

            // Assert
            Assert.DoesNotContain(currency, currencies);
        }

        [Fact]
        public void SupportedBaseCurrencies_HasExpectedCount()
        {
            // Act
            var currencies = CurrencyConst.SupportedBaseCurrencies;

            // Assert
            // Based on the actual implementation: EUR, USD, GBP, MYR, CNY, THB, IDR, INH, BDT, INR, PHP, USDT, MP, NPR, TRY, JPY, MXN, NGN, BRL, PKR, ZAR, KRW
            Assert.Equal(22, currencies.Count());
        }

        [Fact]
        public void SupportedBaseCurrencies_DoesNotContainDuplicates()
        {
            // Act
            var currencies = CurrencyConst.SupportedBaseCurrencies;

            // Assert
            var distinctCurrencies = currencies.Distinct().ToList();
            Assert.Equal(currencies.Count(), distinctCurrencies.Count);
        }

        [Fact]
        public void SupportedBaseCurrencies_IsSameInstanceOnMultipleCalls()
        {
            // Act
            var currencies1 = CurrencyConst.SupportedBaseCurrencies;
            var currencies2 = CurrencyConst.SupportedBaseCurrencies;

            // Assert
            Assert.Same(currencies1, currencies2);
        }
    }
}
