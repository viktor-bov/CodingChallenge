using AlloyOptimisationUtility.Models;
using AlloyOptimisationUtility.Services;

namespace AlloyOptimisationUtilityTests
{
    [Trait("Category", "Unit")]
    public class CompositionGeneratorTests
    {
        private readonly ICompositionGenerator _generator = new CompositionGenerator();

        [Fact]
        public void Generate_RespectsMinimumMaximumAndStep()
        {
            var element = new Element(ElementSymbol.Cr, costCoefficient: 1.0, creepCoefficient: 1.0);
            var system = new AlloySystem(
                new Element(ElementSymbol.Ni, costCoefficient: 1.0),
                new[] { new ElementConstraint(element, minimum: 1.0, maximum: 2.0, step: 0.5) });

            var values = _generator.Generate(system)
                .Select(c => c.Percentages[element])
                .OrderBy(v => v)
                .ToList();

            Assert.Equal(new[] { 1.0, 1.5, 2.0 }, values);
        }

        [Fact]
        public void Generate_DoesNotYieldNegativeBalanceCompositions()
        {
            var element = new Element(ElementSymbol.Cr, costCoefficient: 1.0, creepCoefficient: 1.0);
            var system = new AlloySystem(
                new Element(ElementSymbol.Ni, costCoefficient: 1.0),
                new[] { new ElementConstraint(element, minimum: 0.0, maximum: 150.0, step: 50.0) });

            var compositions = _generator.Generate(system).ToList();

            Assert.All(compositions, c => Assert.True(c.BasePercentage >= 0));
            // 0, 50, 100 are valid; 150 exceeds 100 and must be excluded.
            Assert.Equal(3, compositions.Count);
        }

        [Fact]
        public void Generate_ProducesFloatingPointSafeStepValues()
        {
            var element = new Element(ElementSymbol.Nb, costCoefficient: 1.0, creepCoefficient: 1.0);
            var system = new AlloySystem(
                new Element(ElementSymbol.Ni, costCoefficient: 1.0),
                new[] { new ElementConstraint(element, minimum: 0.0, maximum: 1.5, step: 0.1) });

            var values = _generator.Generate(system)
                .Select(c => c.Percentages[element])
                .ToList();

            // 16 candidate values (0.0 .. 1.5 inclusive); 0.3 must be represented safely.
            Assert.Equal(16, values.Count);
            Assert.Contains(values, v => Math.Abs(v - 0.3) < 1E-9);
        }
    }
}
