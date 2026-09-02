namespace DesignPatternsAndPrinciples
{
    /// <summary>
    /// The set of base machine types the factory knows how to create.
    /// </summary>
    public enum MachinePower
    {
        Low,
        Medium,
        High
    }

    /// <summary>
    /// Creational pattern implementation: Factory Method.
    ///
    /// The factory centralises the creation of the concrete <see cref="AmMachine"/> base
    /// machines so that calling code depends only on the <see cref="AmMachine"/> abstraction
    /// and a simple selector, rather than on the concrete constructors. This keeps object
    /// creation in one place and makes it easy to add new base machine types.
    /// </summary>
    public static class AmMachineFactory
    {
        public static AmMachine Create(MachinePower power) => power switch
        {
            MachinePower.Low => new LowPowerMachine(),
            MachinePower.Medium => new MediumPowerMachine(),
            MachinePower.High => new HighPowerMachine(),
            _ => throw new ArgumentOutOfRangeException(nameof(power), power, "Unknown machine power.")
        };

        /// <summary>
        /// Creates a base machine and wraps it with the requested optional features,
        /// resolving each feature's current name and price from the supplied
        /// <see cref="IFeatureCatalog"/> at runtime.
        ///
        /// This is what enables adding/removing features or changing prices without
        /// recompiling: the caller passes feature keys and a catalog (hardcoded, JSON,
        /// DB, ...) and the factory composes the decorators dynamically.
        /// </summary>
        public static AmMachine Create(
            MachinePower power,
            IFeatureCatalog catalog,
            params IEnumerable<string> featureKeys)
        {
            ArgumentNullException.ThrowIfNull(catalog);

            AmMachine machine = Create(power);

            foreach (var key in featureKeys)
            {
                if (!catalog.TryGet(key, out var feature))
                {
                    throw new ArgumentException($"Unknown feature key '{key}'.", nameof(featureKeys));
                }

                machine = new FeatureDecorator(machine, feature);
            }

            return machine;
        }
    }
}
