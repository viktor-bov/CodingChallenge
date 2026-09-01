namespace AlloyOptimisationUtility.Models
{
    public sealed class Element
    {
        public Element(string symbol, double costCoefficient, double? creepCoefficient = null)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("Element symbol must be provided.", nameof(symbol));
            }

            if (costCoefficient < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(costCoefficient), "Cost coefficient cannot be negative.");
            }

            Symbol = symbol;
            CostCoefficient = costCoefficient;
            CreepCoefficient = creepCoefficient;
        }

        public string Symbol { get; }

        public double CostCoefficient { get; }

        public double? CreepCoefficient { get; }

        public double EffectiveCreepCoefficient => CreepCoefficient ?? 0d;

        public override string ToString() => Symbol;
    }
}
