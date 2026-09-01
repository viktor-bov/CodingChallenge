using AlloyOptimisationUtility;
using AlloyOptimisationUtility.Models;
using AlloyOptimisationUtility.Services;
using AlloyOptimisationUtilityTests.Models;

namespace AlloyOptimisationUtilityTests
{
    [Trait("Category", "Unit")]
    public class AlloyOptimizerTests
    {
        private static IAlloyOptimizer CreateOptimizer(Currency currency, out ICostCalculator costCalculator)
        {
            costCalculator = new CostCalculator(currency);
            return new BruteForceAlloyOptimizer(
                new CompositionGenerator(),
                new CreepResistanceCalculator(),
                costCalculator);
        }

        [Fact]
        public void Optimise_NickelAlloy_FindsExpectedMaximumCreepResistance()
        {
            var system = NickelAlloyFactory.CreateSystem();
            var optimizer = CreateOptimizer(Currency.Gbp, out _);

            var result = optimizer.Optimise(system, NickelAlloyFactory.MaximumCost);

            Assert.NotNull(result);
            Assert.True(result!.SatisfiesConstraints);
            Assert.True(result.Cost <= NickelAlloyFactory.MaximumCost);
            // Expected ~1.72999E18 m^2/s; allow a small floating-point tolerance.
            Assert.Equal(1.72999E18, result.CreepResistance, 1.72999E18 * 1E-4);
        }

        [Fact]
        public void Optimise_ImpossibleBudget_ReturnsNull()
        {
            // Even 100% of the cheapest element (Ni at £8.9) exceeds £1, but the base
            // element cannot be varied to 100 while alloying minima are positive.
            var system = NickelAlloyFactory.CreateSystem();
            var optimizer = CreateOptimizer(Currency.Gbp, out _);

            var result = optimizer.Optimise(system, maximumCost: 1.0);

            Assert.Null(result);
        }

        [Fact]
        public void Optimise_IsGeneric_WorksForNonNickelSystem()
        {
            // A completely different alloy system with a different base and element count.
            var baseElement = new Element("Fe", costCoefficient: 2.0);
            var manganese = new Element("Mn", costCoefficient: 3.0, creepCoefficient: 5.0E15);
            var vanadium = new Element("V", costCoefficient: 25.0, creepCoefficient: 9.0E15);

            var system = new AlloySystem(baseElement, new[]
            {
                new ElementConstraint(manganese, 0.0, 10.0, 1.0),
                new ElementConstraint(vanadium, 0.0, 5.0, 0.5),
            });

            var optimizer = CreateOptimizer(new Currency("USD", "$"), out var costCalculator);

            var result = optimizer.Optimise(system, maximumCost: 5.0);

            Assert.NotNull(result);
            Assert.Equal("USD", result!.Currency.Code);
            Assert.True(result.Cost <= 5.0);
            Assert.Equal(baseElement, result.Composition.BaseElement);
            // Recalculate creep independently to confirm no nickel-specific behaviour.
            var expectedCreep = new CreepResistanceCalculator().Calculate(result.Composition);
            Assert.Equal(expectedCreep, result.CreepResistance, 6);
        }
    }
}
