namespace DesignPatternsAndPrinciples.Pricing
{
    /// <summary>
    /// Mock <see cref="ICurrencyConverter"/> backed by a hardcoded table of rates relative
    /// to a single base currency (GBP by default).
    ///
    /// This stands in for a real FX source. To move to a live feed later, add a new
    /// <see cref="ICurrencyConverter"/> implementation and swap which one is supplied to
    /// the consumer - no other code needs to change.
    /// </summary>
    public class InMemoryCurrencyConverter : ICurrencyConverter
    {
        private readonly Currency _base;

        // Units of each currency per 1 unit of the base currency.
        private readonly IReadOnlyDictionary<string, decimal> _ratesFromBase;

        public InMemoryCurrencyConverter()
            : this(Currency.Gbp, DefaultRates())
        {
        }

        public InMemoryCurrencyConverter(Currency baseCurrency, IReadOnlyDictionary<string, decimal> ratesFromBase)
        {
            _base = baseCurrency ?? throw new ArgumentNullException(nameof(baseCurrency));
            _ratesFromBase = ratesFromBase ?? throw new ArgumentNullException(nameof(ratesFromBase));
        }

        public Money Convert(Money amount, Currency target)
        {
            ArgumentNullException.ThrowIfNull(target);

            if (string.Equals(amount.Currency.Code, target.Code, StringComparison.OrdinalIgnoreCase))
            {
                return amount;
            }

            // Normalise the source amount into the base currency, then into the target.
            decimal inBase = string.Equals(amount.Currency.Code, _base.Code, StringComparison.OrdinalIgnoreCase)
                ? amount.Amount
                : amount.Amount / RateFor(amount.Currency);

            decimal converted = string.Equals(target.Code, _base.Code, StringComparison.OrdinalIgnoreCase)
                ? inBase
                : inBase * RateFor(target);

            return new Money(decimal.Round(converted, 2), target);
        }

        private decimal RateFor(Currency currency)
        {
            if (!_ratesFromBase.TryGetValue(currency.Code, out var rate))
            {
                throw new InvalidOperationException(
                    $"No exchange rate configured for '{currency.Code}' relative to base '{_base.Code}'.");
            }

            return rate;
        }

        // The mocked rates. Replace this method (or the whole class) with a real FX source.
        private static IReadOnlyDictionary<string, decimal> DefaultRates() =>
            new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
            {
                [Currency.Gbp.Code] = 1m,
                [Currency.Usd.Code] = 1.27m,
                [Currency.Eur.Code] = 1.17m,
                [Currency.Jpy.Code] = 191m,
            };
    }
}
