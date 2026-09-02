namespace AlloyOptimisation.Data
{
    using AlloyOptimisationUtility.Models;

    /// <summary>
    /// Thrown when the cost store contains no entry for a requested element symbol. Failing
    /// loudly avoids silently treating a missing price as zero cost.
    /// </summary>
    public sealed class ElementCostNotFoundException : Exception
    {
        public ElementCostNotFoundException(ElementSymbol elementSymbol)
            : base($"No cost was found for element '{elementSymbol}'.")
        {
            ElementSymbol = elementSymbol;
        }

        public ElementSymbol ElementSymbol { get; }
    }
}
