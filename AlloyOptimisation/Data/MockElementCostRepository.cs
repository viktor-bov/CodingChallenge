namespace AlloyOptimisation.Data
{
    using AlloyOptimisationUtility.Models;

    /// <summary>
    /// Hardcoded, in-memory stand-in for the future element-cost database. It exists so the
    /// rest of the application can already depend on <see cref="IElementCostRepository"/>;
    /// replacing this with real database connectivity requires no change to callers.
    /// </summary>
    public sealed class MockElementCostRepository : IElementCostRepository
    {
        // Mock "database rows": element symbol -> cost coefficient (£/kg).
        private static readonly IReadOnlyDictionary<ElementSymbol, double> CostsBySymbol =
            new Dictionary<ElementSymbol, double>
            {
                [ElementSymbol.Ni] = 8.9,
                [ElementSymbol.Cr] = 14.0,
                [ElementSymbol.Co] = 80.5,
                [ElementSymbol.Nb] = 42.5,
                [ElementSymbol.Mo] = 16.0,
            };

        public double GetCostPerKilogram(ElementSymbol elementSymbol)
        {
            if (!Enum.IsDefined(elementSymbol))
            {
                throw new ArgumentException("Element symbol must be a defined value.", nameof(elementSymbol));
            }

            if (!CostsBySymbol.TryGetValue(elementSymbol, out double cost))
            {
                throw new ElementCostNotFoundException(elementSymbol);
            }

            return cost;
        }
    }
}
