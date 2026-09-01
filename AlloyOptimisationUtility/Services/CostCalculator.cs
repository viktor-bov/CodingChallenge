using AlloyOptimisationUtility.Models;

namespace AlloyOptimisationUtility.Services
{
    public sealed class CostCalculator : ICostCalculator
    {
        public CostCalculator(Currency currency)
        {
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }

        public Currency Currency { get; }

        public double Calculate(AlloyComposition composition)
        {
            ArgumentNullException.ThrowIfNull(composition);

            double cost = 0d;
            foreach (var pair in composition.Percentages)
            {
                cost += pair.Key.CostCoefficient * pair.Value / 100d;
            }

            return cost;
        }
    }
}
