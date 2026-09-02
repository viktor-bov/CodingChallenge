namespace AlloyOptimisationUtility.Services
{
    using AlloyOptimisationUtility.Models;

    /// <summary>
    /// Thrown when a cost coefficient cannot be resolved for an element. Failing loudly
    /// prevents a missing external price from silently being treated as zero cost.
    /// </summary>
    public sealed class CostCoefficientNotFoundException : Exception
    {
        public CostCoefficientNotFoundException(ElementSymbol elementSymbol)
            : base($"No cost coefficient was found for element '{elementSymbol}'.")
        {
            ElementSymbol = elementSymbol;
        }

        public ElementSymbol ElementSymbol { get; }
    }
}
