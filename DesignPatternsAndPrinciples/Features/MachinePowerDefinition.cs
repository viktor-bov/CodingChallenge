using DesignPatternsAndPrinciples.Markets;

namespace DesignPatternsAndPrinciples
{
    /// <summary>
    /// A data-only description of a base machine "power" option (e.g. Low/Medium/High
    /// power - or any new tier added later).
    ///
    /// This mirrors <see cref="FeatureDefinition"/>: it decouples the *data* (key, name,
    /// price, wattage, country availability) from the *behaviour* (the <see cref="BaseMachine"/>).
    /// Because base machines are now expressed as data, an array of every possible power
    /// option can be supplied at runtime from any external source - a hardcoded list, a
    /// JSON file, a database, a configuration service, etc. - without recompiling the code
    /// or extending the <see cref="MachinePower"/> enum.
    /// </summary>
    /// <param name="Key">Stable identifier used to select the base machine (e.g. "low-power-machine").</param>
    /// <param name="Name">Human readable display name.</param>
    /// <param name="Cost">Current price of the base machine, in the base currency (GBP).</param>
    /// <param name="PowerWatts">Optional rated laser power in watts (an example of an extra property).</param>
    /// <param name="AvailableCountries">
    /// ISO country codes the base machine may be sold in. An empty/null set means the machine
    /// is available in every country (a global option).
    /// </param>
    public record MachinePowerDefinition(
        string Key,
        string Name,
        decimal Cost,
        int? PowerWatts = null,
        IReadOnlySet<string>? AvailableCountries = null)
    {
        /// <summary>True when this base machine can be sold in the given <paramref name="country"/>.</summary>
        public bool IsAvailableIn(Country country)
        {
            ArgumentNullException.ThrowIfNull(country);

            // No restrictions means the base machine is available everywhere.
            return AvailableCountries is null
                || AvailableCountries.Count == 0
                || AvailableCountries.Contains(country.Code);
        }
    }
}
