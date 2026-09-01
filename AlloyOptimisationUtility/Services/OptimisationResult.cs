using AlloyOptimisationUtility.Models;

namespace AlloyOptimisationUtility.Services
{
    public sealed class OptimisationResult
    {
        public OptimisationResult(
            AlloyComposition composition,
            double creepResistance,
            double cost,
            double maximumCost,
            Currency currency)
        {
            Composition = composition ?? throw new ArgumentNullException(nameof(composition));
            CreepResistance = creepResistance;
            Cost = cost;
            MaximumCost = maximumCost;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }

        public AlloyComposition Composition { get; }

        public double CreepResistance { get; }

        public double Cost { get; }

        public double MaximumCost { get; }

        public Currency Currency { get; }

        public bool SatisfiesConstraints => Cost <= MaximumCost && Composition.BasePercentage >= 0d;
    }
}
