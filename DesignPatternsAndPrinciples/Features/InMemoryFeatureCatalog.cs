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

        public bool TryGet(string key, out FeatureDefinition feature) =>
            _features.TryGetValue(key, out feature!);

        // The mocked data. Replace this method (or the whole class) with a real source.
        private static IEnumerable<FeatureDefinition> DefaultFeatures() =>
        [
            new FeatureDefinition("reduced-build-volume", "Reduced Build Volume", 75_000m),
            new FeatureDefinition("quad-laser", "Quad Laser", 225_000m),
            new FeatureDefinition("powder-recirculation", "Powder Recirculation System", 82_000m),
            new FeatureDefinition("thermal-imaging-camera", "Thermal Imaging Camera", 54_000m),
            new FeatureDefinition("photodiodes", "Photodiodes", 63_000m),
        ];
    }
}
