using System.Diagnostics;

namespace DesignPatternsAndPrinciples.Pricing
{
    /// <summary>
    /// A data-only description of a currency the business can quote prices in.
    ///
    /// Currencies are values (not hardcoded types), so new ones can be added at runtime
    /// from any source without recompiling the pricing or machine code.
    /// </summary>
    /// <param name="Code">ISO 4217 code, e.g. "GBP", "USD", "EUR".</param>
    /// <param name="Symbol">Display symbol, e.g. "£", "$", "€".</param>
    [DebuggerDisplay("{Code}")]
    public record Currency(string Code, string Symbol)
    {
        /// <summary>Pound sterling - the base currency all catalog prices are authored in.</summary>
        public static readonly Currency Gbp = new("GBP", "£");

        public static readonly Currency Usd = new("USD", "$");

        public static readonly Currency Eur = new("EUR", "€");

        public static readonly Currency Jpy = new("JPY", "¥");

        public override string ToString() => Code;
    }
}
