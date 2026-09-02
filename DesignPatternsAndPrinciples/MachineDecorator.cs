namespace DesignPatternsAndPrinciples
{
    /// <summary>
    /// Base class for all optional-feature decorators (Decorator pattern).
    ///
    /// A decorator both wraps and IS an <see cref="AmMachine"/>. It delegates to the
    /// wrapped machine and augments the cost and description with its own optional feature.
    /// Because decorators wrap any <see cref="AmMachine"/> (including other decorators),
    /// features can be composed in arbitrary combinations without subclassing every
    /// machine/feature permutation.
    /// </summary>
    public abstract class MachineDecorator : AmMachine
    {
        protected readonly AmMachine Machine;

        protected MachineDecorator(AmMachine machine)
        {
            Machine = machine ?? throw new ArgumentNullException(nameof(machine));
        }

        /// <summary>The display name and cost of the optional feature this decorator adds.</summary>
        protected abstract string FeatureName { get; }

        protected abstract decimal FeatureCost { get; }

        public override string Description => $"{Machine.Description} + {FeatureName}";

        public override decimal Cost() => Machine.Cost() + FeatureCost;
    }
}
