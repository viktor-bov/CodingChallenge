using AlloyOptimisation.Data;
using AlloyOptimisationUtility.Models;
using AlloyOptimisationUtility.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AlloyOptimisation
{
    public static class Program
    {
        public static void Main()
        {
            ServiceProvider serviceProvider = ConfigureServices();

            var alloyFactory = serviceProvider.GetRequiredService<NickelAlloyFactory>();
            AlloySystem system = alloyFactory.CreateSystem();

            var optimizer = serviceProvider.GetRequiredService<IAlloyOptimizer>();

            OptimisationResult? result = optimizer.Optimise(system, NickelAlloyFactory.MaximumCost);

            if (result is null)
            {
                Console.WriteLine("No valid composition satisfies the cost constraint.");
                return;
            }

            //Console.WriteLine(OptimisationReportFormatter.Format(result));
        }

        private static ServiceProvider ConfigureServices()
        {
            // The cost data comes from a repository abstraction. Today it is an in-memory
            // mock; swapping it for real database connectivity requires no other changes.
            var services = new ServiceCollection();

            services.AddSingleton<IElementCostRepository, MockElementCostRepository>();
            services.AddSingleton<NickelAlloyFactory>();

            services.AddSingleton<ICompositionGenerator, CompositionGenerator>();
            services.AddSingleton<ICreepResistanceCalculator, CreepResistanceCalculator>();
            services.AddSingleton<ICostCalculator>(_ => new CostCalculator(Currency.Gbp));
            services.AddSingleton<IAlloyOptimizer, BruteForceAlloyOptimizer>();

            return services.BuildServiceProvider();
        }
    }
}
