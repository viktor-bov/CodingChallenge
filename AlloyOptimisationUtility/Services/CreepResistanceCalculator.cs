using AlloyOptimisationUtility.Models;

namespace AlloyOptimisationUtility.Services
{
    public sealed class CreepResistanceCalculator : ICreepResistanceCalculator
    {
        public double Calculate(AlloyComposition composition)
        {
            ArgumentNullException.ThrowIfNull(composition);

            double creepResistance = 0d;
            foreach (var pair in composition.Percentages)
            {
                creepResistance += pair.Key.EffectiveCreepCoefficient * pair.Value;
            }

            return creepResistance;
        }
    }
}
