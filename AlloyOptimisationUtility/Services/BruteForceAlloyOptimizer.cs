using AlloyOptimisationUtility.Models;

namespace AlloyOptimisationUtility.Services
{
    public sealed class BruteForceAlloyOptimizer : IAlloyOptimizer
    {
        private readonly ICompositionGenerator _generator;
        private readonly ICreepResistanceCalculator _creepCalculator;
        private readonly ICostCalculator _costCalculator;

        public BruteForceAlloyOptimizer(
            ICompositionGenerator generator,
            ICreepResistanceCalculator creepCalculator,
            ICostCalculator costCalculator)
        {
            _generator = generator ?? throw new ArgumentNullException(nameof(generator));
            _creepCalculator = creepCalculator ?? throw new ArgumentNullException(nameof(creepCalculator));
            _costCalculator = costCalculator ?? throw new ArgumentNullException(nameof(costCalculator));
        }

        public OptimisationResult? Optimise(AlloySystem system, double maximumCost)
        {
            ArgumentNullException.ThrowIfNull(system);

            AlloyComposition? best = null;
            double bestCreep = double.NegativeInfinity;
            double bestCost = 0d;

            foreach (var composition in _generator.Generate(system))
            {
                // Reject compositions that exceed the cost budget before doing further work.
                double cost = _costCalculator.Calculate(composition);
                if (cost > maximumCost)
                {
                    continue;
                }

                double creep = _creepCalculator.Calculate(composition);
                if (creep > bestCreep)
                {
                    bestCreep = creep;
                    best = composition;
                    bestCost = cost;
                }
            }

            if (best is null)
            {
                return null;
            }

            return new OptimisationResult(best, bestCreep, bestCost, maximumCost, _costCalculator.Currency);
        }
    }
}
