using AlloyOptimisationUtility.Models;

namespace AlloyOptimisationUtilityTests.Models
{
    public static class NickelAlloyFactory
    {
        // Creep coefficients (alpha) in m^2/s per atomic percent.
        private const double CrCreepCoefficient = 2.0911350E+16;
        private const double CoCreepCoefficient = 7.2380280E+16;
        private const double NbCreepCoefficient = 1.0352738E+16;
        private const double MoCreepCoefficient = 8.9124547E+16;

        // Cost coefficients (c) in £/kg.
        private const double CrCost = 14.0;
        private const double CoCost = 80.5;
        private const double NbCost = 42.5;
        private const double MoCost = 16.0;
        private const double NiCost = 8.9;

        public const double MaximumCost = 18.0;

        public static AlloySystem CreateSystem()
        {
            // Nickel is the base element and therefore has no creep coefficient.
            var nickel = new Element("Ni", NiCost);

            var chromium = new Element("Cr", CrCost, CrCreepCoefficient);
            var cobalt = new Element("Co", CoCost, CoCreepCoefficient);
            var niobium = new Element("Nb", NbCost, NbCreepCoefficient);
            var molybdenum = new Element("Mo", MoCost, MoCreepCoefficient);

            var constraints = new[]
            {
                new ElementConstraint(chromium, minimum: 14.5, maximum: 22.0, step: 0.50),
                new ElementConstraint(cobalt, minimum: 0.0, maximum: 25.0, step: 1.00),
                new ElementConstraint(niobium, minimum: 0.0, maximum: 1.5, step: 0.10),
                new ElementConstraint(molybdenum, minimum: 1.5, maximum: 6.0, step: 0.50),
            };

            return new AlloySystem(nickel, constraints);
        }
    }
}
