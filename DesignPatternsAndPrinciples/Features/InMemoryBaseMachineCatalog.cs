using DesignPatternsAndPrinciples.Markets;

namespace DesignPatternsAndPrinciples
{
    /// <summary>
    /// Mock <see cref="IBaseMachineCatalog"/> backed by a hardcoded list.
    ///
    /// This stands in for a real data source. To supply base machines from an external
    /// source, either construct this class with your own array of
    /// <see cref="MachinePowerDefinition"/> (e.g. deserialized from JSON/DB/config) or add a
    /// new <see cref="IBaseMachineCatalog"/> implementation (e.g. <c>JsonBaseMachineCatalog</c>)
    /// and swap which one is supplied to the factory - no other code needs to change, and no
    /// new entries are required in the <see cref="MachinePower"/> enum.
    /// </summary>
    public class InMemoryBaseMachineCatalog : IBaseMachineCatalog
    {
        private readonly IReadOnlyDictionary<string, MachinePowerDefinition> _machines;

        public InMemoryBaseMachineCatalog()
            : this(DefaultBaseMachines())
        {
        }

        public InMemoryBaseMachineCatalog(IEnumerable<MachinePowerDefinition> machines)
        {
            ArgumentNullException.ThrowIfNull(machines);
            _machines = machines.ToDictionary(m => m.Key, StringComparer.OrdinalIgnoreCase);
        }

        public IReadOnlyCollection<MachinePowerDefinition> GetAll() => _machines.Values.ToList();

        public IReadOnlyCollection<MachinePowerDefinition> GetAvailableIn(Country country)
        {
            ArgumentNullException.ThrowIfNull(country);
            return _machines.Values.Where(m => m.IsAvailableIn(country)).ToList();
        }

        public bool TryGet(string key, out MachinePowerDefinition definition) =>
            _machines.TryGetValue(key, out definition!);

        // The mocked data. Replace this method (or the whole class) with a real source.
        // Base machines with no country set are global (available everywhere); a non-empty
        // set restricts the machine to specific export markets - exactly like features.
        private static IEnumerable<MachinePowerDefinition> DefaultBaseMachines() =>
        [
            new MachinePowerDefinition("low-power-machine", "Low Power Machine (200W)", 450_000m, PowerWatts: 200),
            new MachinePowerDefinition("medium-power-machine", "Medium Power Machine (400W)", 550_000m, PowerWatts: 400),
            new MachinePowerDefinition("high-power-machine", "High Power Machine (500W)", 750_000m, PowerWatts: 500),
        ];
    }
}
