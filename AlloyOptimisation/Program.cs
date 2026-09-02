using AlloyOptimisation.Data;
using AlloyOptimisationUtility.Models;
using AlloyOptimisationUtility.Services;

namespace AlloyOptimisation
{
    public static class Program
    {
        public static void Main()
        {
            // The cost data comes from a repository abstraction. Today it is an in-memory
            // mock; swapping it for real database connectivity requires no other changes.
            IElementCostRepository costRepository = new MockElementCostRepository();
            var alloyFactory = new NickelAlloyFactory(costRepository);

            AlloySystem system = alloyFactory.CreateSystem();

            ICompositionGenerator generator = new CompositionGenerator();
            ICreepResistanceCalculator creepCalculator = new CreepResistanceCalculator();
            ICostCalculator costCalculator = new CostCalculator(Currency.Gbp);
            IAlloyOptimizer optimizer = new BruteForceAlloyOptimizer(generator, creepCalculator, costCalculator);

            OptimisationResult? result = optimizer.Optimise(system, NickelAlloyFactory.MaximumCost);

            if (result is null)
            {
                Console.WriteLine("No valid composition satisfies the cost constraint.");
                return;
            }

            //Console.WriteLine(OptimisationReportFormatter.Format(result));
        }
    }
}
