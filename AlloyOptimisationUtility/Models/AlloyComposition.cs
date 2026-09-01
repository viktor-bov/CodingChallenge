namespace AlloyOptimisationUtility.Models
{
    public sealed class AlloyComposition
    {
        private readonly IReadOnlyDictionary<Element, double> _percentages;

        private AlloyComposition(
            Element baseElement,
            double basePercentage,
            IReadOnlyDictionary<Element, double> percentages)
        {
            BaseElement = baseElement;
            BasePercentage = basePercentage;
            _percentages = percentages;
        }

        public Element BaseElement { get; }

        public double BasePercentage { get; }

        public IReadOnlyDictionary<Element, double> Percentages => _percentages;

        public double TotalPercentage => _percentages.Values.Sum();

        /// <summary>
        /// Creates a composition from the alloying-element percentages, computing the base
        /// element as the balance. Returns <c>null</c> when the balance would be negative,
        /// which keeps invalid compositions out of the search results.
        /// </summary>
        public static AlloyComposition? TryCreate(
            AlloySystem system,
            IReadOnlyList<double> alloyingPercentages)
        {
            ArgumentNullException.ThrowIfNull(system);
            ArgumentNullException.ThrowIfNull(alloyingPercentages);

            if (alloyingPercentages.Count != system.AlloyElementsWithConstraints.Count)
            {
                throw new ArgumentException("Percentage count must match the number of alloying constraints.", nameof(alloyingPercentages));
            }

            double alloyingSum = 0d;
            var percentages = new Dictionary<Element, double>(alloyingPercentages.Count + 1);

            for (int i = 0; i < alloyingPercentages.Count; i++)
            {
                double value = alloyingPercentages[i];
                alloyingSum += value;
                percentages[system.AlloyElementsWithConstraints[i].Element] = value;
            }

            double basePercentage = 100d - alloyingSum;

            // A negative base concentration is physically impossible, so reject it.
            if (basePercentage < 0d)
            {
                return null;
            }

            percentages[system.BaseElement] = basePercentage;

            return new AlloyComposition(system.BaseElement, basePercentage, percentages);
        }
    }
}
