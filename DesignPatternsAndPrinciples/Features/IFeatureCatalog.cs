using DesignPatternsAndPrinciples.Markets;

namespace DesignPatternsAndPrinciples
{
    /// <summary>
    /// Supplies the set of optional features (and their current prices) available at runtime.
    ///
    /// This is the extension seam that lets prices/features change without recompiling:
    /// implementations can read from a hardcoded list, a JSON file, a database, a remote
    /// configuration service, etc. Calling code depends only on this abstraction.
    /// </summary>
    public interface IFeatureCatalog
    {
        /// <summary>Returns every feature currently offered.</summary>
        IReadOnlyCollection<FeatureDefinition> GetAll();

        /// <summary>Returns only the features that may be sold in the given country.</summary>
        IReadOnlyCollection<FeatureDefinition> GetAvailableIn(Country country);

        /// <summary>
        /// Attempts to resolve a single feature by its stable <see cref="FeatureDefinition.Key"/>.
        /// </summary>
        bool TryGet(string key, out FeatureDefinition feature);
    }
}
