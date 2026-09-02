namespace DesignPatternsAndPrinciples
{
    /// <summary>
    /// A generic <see cref="MachineDecorator"/> driven entirely by a <see cref="FeatureDefinition"/>.
    ///
    /// Because the name and cost come from data (not a hardcoded subclass), new features
    /// can be added, removed or re-priced at runtime by changing the catalog data alone -
    /// no new decorator class or recompilation required.
    /// </summary>
    public class FeatureDecorator : MachineDecorator
    {
        private readonly FeatureDefinition _feature;

        public FeatureDecorator(AmMachine machine, FeatureDefinition feature) : base(machine)
        {
            _feature = feature ?? throw new ArgumentNullException(nameof(feature));
        }

        protected override string FeatureName => _feature.Name;

        protected override decimal FeatureCost => _feature.Cost;
    }
}
