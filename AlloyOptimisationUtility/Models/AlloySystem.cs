namespace AlloyOptimisationUtility.Models
{
    public sealed class AlloySystem
    {
        private readonly List<ElementConstraint> _alloyElementsWithConstraints;

        public AlloySystem(Element baseElement, IEnumerable<ElementConstraint> alloyElementsWithConstraints)
        {
            BaseElement = baseElement ?? throw new ArgumentNullException(nameof(baseElement));

            if (alloyElementsWithConstraints is null)
            {
                throw new ArgumentNullException(nameof(alloyElementsWithConstraints));
            }

            _alloyElementsWithConstraints = alloyElementsWithConstraints.ToList();

            if (_alloyElementsWithConstraints.Count == 0)
            {
                throw new ArgumentException("At least one alloying element is required.", nameof(alloyElementsWithConstraints));
            }

            if (_alloyElementsWithConstraints.Any(c => ReferenceEquals(c.Element, baseElement) || c.Element.Symbol == baseElement.Symbol))
            {
                throw new ArgumentException("The base element must not also be supplied as an alloying element.", nameof(alloyElementsWithConstraints));
            }
        }

        public Element BaseElement { get; }

        public IReadOnlyList<ElementConstraint> AlloyElementsWithConstraints => _alloyElementsWithConstraints;
    }
}
