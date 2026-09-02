namespace AlloyOptimisationUtility.Models
{
    public sealed class Element
    {
        public Element(ElementSymbol symbol, double costCoefficient, double? creepCoefficient = null)
        {
            if (!Enum.IsDefined(symbol))
            {
                throw new ArgumentException("Element symbol must be a defined value.", nameof(symbol));
            }

            if (costCoefficient < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(costCoefficient), "Cost coefficient cannot be negative.");
            }

            Symbol = symbol;
            CostCoefficient = costCoefficient;
            CreepCoefficient = creepCoefficient;
        }

        public ElementSymbol Symbol { get; }

        /// <summary>The element's atomic number in the periodic table.</summary>
        public int AtomicNumber => (int)Symbol;

        public double CostCoefficient { get; }

        public double? CreepCoefficient { get; }

        public double EffectiveCreepCoefficient => CreepCoefficient ?? 0d;

        public override string ToString() => Symbol.ToString();
    }
}
