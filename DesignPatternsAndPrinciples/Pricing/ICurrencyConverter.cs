namespace DesignPatternsAndPrinciples.Pricing
{
    /// <summary>
    /// Converts a money amount from one currency to another.
    ///
    /// This is the extension seam that decouples pricing from any specific exchange-rate
    /// source: an implementation can use a hardcoded table, a cached daily feed, or a live
    /// FX service. Calling code depends only on this abstraction (DIP), so the source can
    /// be swapped without recompiling machine, feature or factory code.
    /// </summary>
    public interface ICurrencyConverter
    {
        /// <summary>
        /// Converts <paramref name="amount"/> into <paramref name="target"/> currency.
        /// Returns the amount unchanged when it is already in the target currency.
        /// </summary>
        Money Convert(Money amount, Currency target);
    }
}
