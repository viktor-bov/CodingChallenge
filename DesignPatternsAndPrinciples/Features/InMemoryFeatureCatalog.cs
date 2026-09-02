using DesignPatternsAndPrinciples.Markets;

namespace DesignPatternsAndPrinciples
{
    /// <summary>
    /// Mock <see cref="IFeatureCatalog"/> backed by a hardcoded list.
    ///
    /// This stands in for a real data source. To move to JSON/DB/configuration later,
    /// simply add a new <see cref="IFeatureCatalog"/> implementation (e.g.
    /// <c>JsonFeatureCatalog</c> or <c>SqlFeatureCatalog</c>) and swap which one is
    /// supplied to the consumer - no other code needs to change.
    /// </summary>
    public class InMemoryFeatureCatalog : IFeatureCatalog
    {
        private readonly IReadOnlyDictionary<string, FeatureDefinition> _features;

        public InMemoryFeatureCatalog()
            : this(DefaultFeatures())
        {
        }

        public InMemoryFeatureCatalog(IEnumerable<FeatureDefinition> features)
        {
            _features = features.ToDictionary(f => f.Key, StringComparer.OrdinalIgnoreCase);
        }

        public IReadOnlyCollection<FeatureDefinition> GetAll() => _features.Values.ToList();

        public IReadOnlyCollection<FeatureDefinition> GetAvailableIn(Country country)
        {
            ArgumentNullException.ThrowIfNull(country);
            return _features.Values.Where(f => f.IsAvailableIn(country)).ToList();
        }

        public bool TryGet(string key, out FeatureDefinition feature) =>
            _features.TryGetValue(key, out feature!);

        // The mocked data. Replace this method (or the whole class) with a real source.
        // Features with no country set are global (available everywhere); a non-empty set
        // restricts the feature to specific export markets.
        //
        // Base machines are authored separately as data in an IBaseMachineCatalog (see
        // InMemoryBaseMachineCatalog); this catalog holds only the optional features.
        private static IEnumerable<FeatureDefinition> DefaultFeatures() =>
        [
            new FeatureDefinition("reduced-build-volume", "Reduced Build Volume", 75_000m),
            new FeatureDefinition("quad-laser", "Quad Laser", 225_000m),
            new FeatureDefinition("powder-recirculation", "Powder Recirculation System", 82_000m),
            new FeatureDefinition("thermal-imaging-camera", "Thermal Imaging Camera", 54_000m),
            new FeatureDefinition("photodiodes", "Photodiodes", 63_000m),
            // Example of a country-restricted feature: only sold in the United States.
            new FeatureDefinition(
                "high-power-export-pack",
                "High Power Export Pack",
                48_000m,
                new HashSet<string>(StringComparer.OrdinalIgnoreCase) { Country.UnitedStates.Code }),
        ];
    }
}
