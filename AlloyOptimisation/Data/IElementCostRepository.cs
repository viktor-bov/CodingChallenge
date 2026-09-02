namespace AlloyOptimisation.Data
{
    using AlloyOptimisationUtility.Models;

    /// <summary>
    /// Read-only access to the persisted market cost data for elements (price per kilogram).
    /// <para>
    /// Cost figures are business/market data that live outside the application, so they are
    /// retrieved through this abstraction rather than being compiled into the alloy model.
    /// The current implementation is an in-memory stand-in, but it can be swapped for a
    /// database-backed implementation (e.g. EF Core, Dapper, an HTTP price service) without
    /// changing any consumer.
    /// </para>
    /// </summary>
    public interface IElementCostRepository
    {
        /// <summary>
        /// Returns the current cost coefficient (price per kilogram) for the element with the
        /// given <paramref name="elementSymbol"/>.
        /// </summary>
        /// <exception cref="System.ArgumentException"><paramref name="elementSymbol"/> is not a defined value.</exception>
        /// <exception cref="ElementCostNotFoundException">No cost is stored for the symbol.</exception>
        double GetCostPerKilogram(ElementSymbol elementSymbol);
    }
}
