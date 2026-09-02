using DesignPatternsAndPrinciples.Markets;

namespace DesignPatternsAndPrinciples
{
    /// <summary>
    /// Supplies the set of base machine power options (and their current prices) available
    /// at runtime.
    ///
    /// This is the extension seam that lets new base machines be added without recompiling:
    /// implementations can read the full array of <see cref="MachinePowerDefinition"/> from a
    /// hardcoded list, a JSON file, a database, a remote configuration service, etc. Calling
    /// code depends only on this abstraction and a string key.
    /// </summary>
    public interface IBaseMachineCatalog
    {
        /// <summary>Returns every base machine power option currently offered.</summary>
        IReadOnlyCollection<MachinePowerDefinition> GetAll();

        /// <summary>Returns only the base machines that may be sold in the given country.</summary>
        IReadOnlyCollection<MachinePowerDefinition> GetAvailableIn(Country country);

        /// <summary>
        /// Attempts to resolve a single base machine by its stable
        /// <see cref="MachinePowerDefinition.Key"/>.
        /// </summary>
        bool TryGet(string key, out MachinePowerDefinition definition);
    }
}
