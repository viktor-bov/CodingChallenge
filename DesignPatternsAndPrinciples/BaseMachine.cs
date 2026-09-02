namespace DesignPatternsAndPrinciples
{
    /// <summary>
    /// A base <see cref="AmMachine"/> whose name and cost are driven entirely by a
    /// <see cref="FeatureDefinition"/> supplied from an <see cref="IFeatureCatalog"/>.
    ///
    /// Because the base machines (Low/Medium/High power) are now data rather than
    /// hardcoded subclasses, they can be added, removed or re-priced at runtime by
    /// changing the catalog data alone - no new machine class or recompilation required.
    /// </summary>
    public class BaseMachine : AmMachine
    {
        private readonly string _name;
        private readonly decimal _cost;

        public BaseMachine(MachinePowerDefinition definition)
        {
            ArgumentNullException.ThrowIfNull(definition);
            _name = definition.Name;
            _cost = definition.Cost;
        }

        public override string Description => _name;

        public override decimal Cost() => _cost;
    }
}
