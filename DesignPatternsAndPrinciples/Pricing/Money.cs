using System.Diagnostics;
using System.Globalization;

namespace DesignPatternsAndPrinciples.Pricing
{
    /// <summary>
    /// An amount of money in a specific <see cref="Currency"/>.
    ///
    /// Pairing the amount with its currency prevents accidentally mixing currencies and
    /// makes every price self-describing when a machine is exported to another country.
    /// </summary>
    /// <param name="Amount">The numeric amount.</param>
    /// <param name="Currency">The currency the amount is expressed in.</param>
    [DebuggerDisplay("{Currency.Symbol}{Amount} {Currency.Code}")]
    public readonly record struct Money(decimal Amount, Currency Currency)
    {
        /// <summary>Formats the amount with its currency symbol, e.g. "£932,000.00".</summary>
        public override string ToString() =>
            $"{Currency.Symbol}{Amount.ToString("N2", CultureInfo.InvariantCulture)} {Currency.Code}";
    }
}
