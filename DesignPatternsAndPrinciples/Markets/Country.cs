using System.Diagnostics;
using DesignPatternsAndPrinciples.Pricing;

namespace DesignPatternsAndPrinciples.Markets
{
    /// <summary>
    /// A country the business exports machines to, together with the currency prices are
    /// quoted in there by default.
    ///
    /// Countries are data (not hardcoded types), so new export markets can be added at
    /// runtime without recompiling the machine, feature or pricing code.
    /// </summary>
    /// <param name="Code">ISO 3166-1 alpha-2 code, e.g. "GB", "US", "DE".</param>
    /// <param name="Name">Human readable country name.</param>
    /// <param name="DefaultCurrency">Currency prices are quoted in for this country by default.</param>
    [DebuggerDisplay("{Name} ({Code})")]
    public record Country(string Code, string Name, Currency DefaultCurrency)
    {
        public static readonly Country UnitedKingdom = new("GB", "United Kingdom", Currency.Gbp);

        public static readonly Country UnitedStates = new("US", "United States", Currency.Usd);

        public static readonly Country Germany = new("DE", "Germany", Currency.Eur);

        public static readonly Country Japan = new("JP", "Japan", Currency.Jpy);

        public override string ToString() => $"{Name} ({Code})";
    }
}
