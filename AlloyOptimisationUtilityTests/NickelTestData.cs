using AlloyOptimisationUtility.Models;

namespace AlloyOptimisationUtilityTests
{
    /// <summary>
    /// Shared nickel elements used across several test fixtures. The base element
    /// deliberately has no creep coefficient.
    /// </summary>
    internal static class NickelTestData
    {
        public static Element Nickel { get; } = new(ElementSymbol.Ni, costCoefficient: 8.9);
        public static Element Chromium { get; } = new(ElementSymbol.Cr, costCoefficient: 14.0, creepCoefficient: 2.0911350E+16);
        public static Element Cobalt { get; } = new(ElementSymbol.Co, costCoefficient: 80.5, creepCoefficient: 7.2380280E+16);
        public static Element Niobium { get; } = new(ElementSymbol.Nb, costCoefficient: 42.5, creepCoefficient: 1.0352738E+16);
        public static Element Molybdenum { get; } = new(ElementSymbol.Mo, costCoefficient: 16.0, creepCoefficient: 8.9124547E+16);

        /// <summary>The full nickel-alloy composition search space.</summary>
        public static AlloySystem CreateSystem()
        {
            return new AlloySystem(Nickel, new[]
            {
                new ElementConstraint(Chromium, 14.5, 22.0, 0.5),
                new ElementConstraint(Cobalt, 0.0, 25.0, 1.0),
                new ElementConstraint(Niobium, 0.0, 1.5, 0.1),
                new ElementConstraint(Molybdenum, 1.5, 6.0, 0.5),
            });
        }

        /// <summary>Creates a composition from Cr/Co/Nb/Mo percentages (Ni is the balance).</summary>
        public static AlloyComposition Create(double cr, double co, double nb, double mo)
        {
            var composition = AlloyComposition.TryCreate(CreateSystem(), new[] { cr, co, nb, mo });
            Assert.NotNull(composition);
            return composition!;
        }
    }
}
