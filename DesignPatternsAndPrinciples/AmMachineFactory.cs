namespace DesignPatternsAndPrinciples
{
    using DesignPatternsAndPrinciples.Markets;
    using DesignPatternsAndPrinciples.Pricing;

    /// <summary>
    /// Creational pattern implementation: Factory Method.
    ///
    /// The factory centralises the creation of the concrete <see cref="AmMachine"/> base
    /// machines so that calling code depends only on the <see cref="AmMachine"/> abstraction
    /// and a stable string key, rather than on the concrete constructors. Because both the
    /// base machines and their optional features are now pure data, new base machine power
    /// options and features can be supplied from external sources (JSON/DB/config) without
    /// recompiling.
    /// </summary>
    public static class AmMachineFactory
    {
        /// <summary>
        /// Creates a base machine (by stable string <paramref name="powerKey"/>) using the
        /// default <see cref="InMemoryBaseMachineCatalog"/> as the data source.
        /// </summary>
        public static AmMachine Create(string powerKey) =>
            CreateBase(powerKey, new InMemoryBaseMachineCatalog());

        /// <summary>
        /// Creates a base machine by resolving its <see cref="MachinePowerDefinition"/> from the
        /// supplied <paramref name="baseMachines"/> catalog using a stable string
        /// <paramref name="powerKey"/>.
        ///
        /// This is the extension point that lets brand new base machine power options be added
        /// purely as data (e.g. loaded from JSON/DB/config) without recompiling.
        /// </summary>
        public static BaseMachine CreateBase(string powerKey, IBaseMachineCatalog baseMachines)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(powerKey);
            ArgumentNullException.ThrowIfNull(baseMachines);

            if (!baseMachines.TryGet(powerKey, out var definition))
            {
                throw new ArgumentException($"Unknown base machine key '{powerKey}'.", nameof(powerKey));
            }

            return new BaseMachine(definition);
        }

        /// <summary>
        /// Creates a base machine from an <see cref="IBaseMachineCatalog"/> (selected by a
        /// stable string <paramref name="powerKey"/>) and wraps it with the requested optional
        /// features resolved from the <see cref="IFeatureCatalog"/>.
        ///
        /// Both the base machine tiers and the features are pure data, so new power options
        /// and features can be supplied from external sources without recompiling.
        /// </summary>
        public static AmMachine Create(
            string powerKey,
            IBaseMachineCatalog baseMachines,
            IFeatureCatalog catalog,
            params IEnumerable<string> featureKeys)
        {
            ArgumentNullException.ThrowIfNull(catalog);

            AmMachine machine = CreateBase(powerKey, baseMachines);

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

        /// <summary>
        /// Country-aware counterpart to <see cref="Create(string, IBaseMachineCatalog, IFeatureCatalog, IEnumerable{string})"/>.
        ///
        /// The base machine itself must be available in <paramref name="country"/> (base machines
        /// carry <see cref="MachinePowerDefinition.AvailableCountries"/> just like features), and
        /// every requested feature must also be sold in that market.
        /// </summary>
        public static AmMachine CreateForCountry(
            string powerKey,
            IBaseMachineCatalog baseMachines,
            IFeatureCatalog catalog,
            Country country,
            params IEnumerable<string> featureKeys)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(powerKey);
            ArgumentNullException.ThrowIfNull(baseMachines);
            ArgumentNullException.ThrowIfNull(catalog);
            ArgumentNullException.ThrowIfNull(country);

            if (!baseMachines.TryGet(powerKey, out var baseDefinition))
            {
                throw new ArgumentException($"Unknown base machine key '{powerKey}'.", nameof(powerKey));
            }

            if (!baseDefinition.IsAvailableIn(country))
            {
                throw new ArgumentException(
                    $"Base machine '{baseDefinition.Name}' is not available in {country}.",
                    nameof(powerKey));
            }

            AmMachine machine = new BaseMachine(baseDefinition);

            foreach (var key in featureKeys)
            {
                if (!catalog.TryGet(key, out var feature))
                {
                    throw new ArgumentException($"Unknown feature key '{key}'.", nameof(featureKeys));
                }

                if (!feature.IsAvailableIn(country))
                {
                    throw new ArgumentException(
                        $"Feature '{feature.Name}' is not available in {country}.",
                        nameof(featureKeys));
                }

                machine = new FeatureDecorator(machine, feature);
            }

            return machine;
        }

        /// <summary>
        /// Produces a localised <see cref="MachineQuote"/> for exporting the given
        /// <paramref name="machine"/> to <paramref name="country"/>.
        ///
        /// Catalog prices are authored in the base currency (GBP); the machine's total cost
        /// is converted into the quote currency via the injected <see cref="ICurrencyConverter"/>
        /// (DIP), so the FX source can be swapped without touching machine or feature code.
        /// When <paramref name="quoteCurrency"/> is null the country's default currency is used.
        /// </summary>
        public static MachineQuote Quote(
            AmMachine machine,
            Country country,
            ICurrencyConverter converter,
            Currency? quoteCurrency = null)
        {
            ArgumentNullException.ThrowIfNull(machine);
            ArgumentNullException.ThrowIfNull(country);
            ArgumentNullException.ThrowIfNull(converter);

            var basePrice = new Money(machine.Cost(), Currency.Gbp);
            var target = quoteCurrency ?? country.DefaultCurrency;
            var localPrice = converter.Convert(basePrice, target);

            return new MachineQuote(machine.Description, country, basePrice, localPrice);
        }
    }
}
