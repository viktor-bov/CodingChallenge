using System.Diagnostics;
using DesignPatternsAndPrinciples.Markets;

namespace DesignPatternsAndPrinciples.Pricing
{
    /// <summary>
    /// The result of pricing a composed <see cref="AmMachine"/> for a specific export market.
    ///
    /// Carries the machine description, the destination country, the base (GBP) price the
    /// catalog was authored in, and the localised price actually quoted to the customer.
    /// </summary>
    /// <param name="Description">The composed machine description (base machine + features).</param>
    /// <param name="Country">The destination export market.</param>
    /// <param name="BasePrice">The price in the base authoring currency (GBP).</param>
    /// <param name="LocalPrice">The price converted into the quote currency for the country.</param>
    [DebuggerDisplay("{Description} for {Country}: {LocalPrice} (base {BasePrice})")]
    public record MachineQuote(string Description, Country Country, Money BasePrice, Money LocalPrice)
    {
        public override string ToString() =>
            $"{Description} for {Country}: {LocalPrice} (base {BasePrice})";
    }
}
