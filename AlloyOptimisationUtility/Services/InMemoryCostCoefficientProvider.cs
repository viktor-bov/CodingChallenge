using AlloyOptimisationUtility.Models;

namespace AlloyOptimisationUtility.Services
{
    /// <summary>
    /// In-memory, read-only <see cref="ICostCoefficientProvider"/> used as a stand-in for an
    /// external price source. It is seeded from a supplied table of symbol -&gt; cost and can be
    /// replaced by a database-backed implementation without touching any consumer.
    /// </summary>
    /// <remarks>
    /// Security notes:
    /// <list type="bullet">
    /// <item>The internal table is copied and never exposed, so callers cannot mutate prices.</item>
    /// <item>Lookups are case-sensitive on the trusted element symbol only; no free-form input
    /// is used to build queries, avoiding injection-style risks when this is swapped for a
    /// database implementation.</item>
    /// <item>A missing coefficient throws rather than defaulting to zero, preventing a silent
    /// "free element" that could corrupt optimisation results.</item>
    /// </list>
    /// </remarks>
    public sealed class InMemoryCostCoefficientProvider : ICostCoefficientProvider
    {
        private readonly IReadOnlyDictionary<ElementSymbol, double> _coefficientsBySymbol;

        public InMemoryCostCoefficientProvider(IReadOnlyDictionary<ElementSymbol, double> coefficientsBySymbol)
        {
            ArgumentNullException.ThrowIfNull(coefficientsBySymbol);

            var copy = new Dictionary<ElementSymbol, double>(coefficientsBySymbol.Count);
            foreach (var pair in coefficientsBySymbol)
            {
                if (!Enum.IsDefined(pair.Key))
                {
                    throw new ArgumentException("Element symbols must be defined values.", nameof(coefficientsBySymbol));
                }

                if (pair.Value < 0d || double.IsNaN(pair.Value) || double.IsInfinity(pair.Value))
                {
                    throw new ArgumentException(
                        $"Cost coefficient for '{pair.Key}' must be a non-negative, finite value.",
                        nameof(coefficientsBySymbol));
                }

                copy[pair.Key] = pair.Value;
            }

            _coefficientsBySymbol = copy;
        }

        public double GetCostCoefficient(Element element)
        {
            ArgumentNullException.ThrowIfNull(element);

            if (!_coefficientsBySymbol.TryGetValue(element.Symbol, out double coefficient))
            {
                throw new CostCoefficientNotFoundException(element.Symbol);
            }

            return coefficient;
        }
    }
}
