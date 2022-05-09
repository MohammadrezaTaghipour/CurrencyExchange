using CurrencyExchange.Persistence.Graph;
using FluentAssertions;
using Xunit;

namespace CurrencyExchange.Domain.Tests.Unit.CurrencyConverter
{
    public class When_converting_currencies
    {
        [Theory]
        [InlineData("USD", "CAD", 1, 1.29)]
        [InlineData("CAD", "USD", 1, 0.77)]
        [InlineData("CAD", "GBP", 1, 0.62)]
        [InlineData("GBP", "CAD", 1, 1.61)]
        [InlineData("USD", "EUR", 1, 0.95)]
        [InlineData("EUR", "USD", 1, 1.05)]
        [InlineData("CAD", "EUR", 1, 0.73)]
        [InlineData("GBP", "EUR", 1, 1.18)]
        public void It_converts_currencies_properly(string fromCurrency, string toCurrency, double amount, double expected)
        {
            var currencies = new[]
            {
                Tuple.Create("USD", "CAD", 1.29),
                Tuple.Create("CAD","GBP", 0.62),
                Tuple.Create("USD","EUR", 0.95)
            };

            var graphRepository = new InMemoryGraphRepository();
            var currencyConverter = new Domain.CurrencyConverter(graphRepository);
            currencyConverter.UpdateConfiguration(currencies);

            var actual = currencyConverter.Convert(fromCurrency, toCurrency, amount);

            actual.Should().BeApproximately(expected, 0.01F);
        }
    }
}
