using AlloyOptimisation.Data;
using AlloyOptimisationUtility.Models;

namespace AlloyOptimisation
{
    /// <summary>
    /// Builds the nickel-based <see cref="AlloySystem"/> to be optimised. Creep coefficients
    /// and concentration constraints are material-physics constants and stay here, but the
    /// element cost coefficients are treated as external market data and are resolved through
    /// an <see cref="IElementCostRepository"/> (currently a mock, later a real database).
    /// </summary>
    public sealed class NickelAlloyFactory
    {
        // Creep coefficients (alpha) in m^2/s per atomic percent.
        private const double CrCreepCoefficient = 2.0911350E+16;
        private const double CoCreepCoefficient = 7.2380280E+16;
        private const double NbCreepCoefficient = 1.0352738E+16;
        private const double MoCreepCoefficient = 8.9124547E+16;

        public const double MaximumCost = 18.0;

        private readonly IElementCostRepository _costRepository;

        public NickelAlloyFactory(IElementCostRepository costRepository)
        {
            _costRepository = costRepository ?? throw new ArgumentNullException(nameof(costRepository));
        }

        public AlloySystem CreateSystem()
        {
            // Nickel is the base element and therefore has no creep coefficient.
            var nickel = CreateElement(ElementSymbol.Ni);

            var chromium = CreateElement(ElementSymbol.Cr, CrCreepCoefficient);
            var cobalt = CreateElement(ElementSymbol.Co, CoCreepCoefficient);
            var niobium = CreateElement(ElementSymbol.Nb, NbCreepCoefficient);
            var molybdenum = CreateElement(ElementSymbol.Mo, MoCreepCoefficient);

            var constraints = new[]
            {
                new ElementConstraint(chromium, minimum: 14.5, maximum: 22.0, step: 0.50),
                new ElementConstraint(cobalt, minimum: 0.0, maximum: 25.0, step: 1.00),
                new ElementConstraint(niobium, minimum: 0.0, maximum: 1.5, step: 0.10),
                new ElementConstraint(molybdenum, minimum: 1.5, maximum: 6.0, step: 0.50),
            };

            return new AlloySystem(nickel, constraints);
        }

        private Element CreateElement(ElementSymbol symbol, double? creepCoefficient = null)
        {
            double cost = _costRepository.GetCostPerKilogram(symbol);
            return new Element(symbol, cost, creepCoefficient);
        }
    }
}
