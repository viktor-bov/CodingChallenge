using DesignPatternsAndPrinciples.Markets;

namespace DesignPatternsAndPrinciples
{
    /// <summary>
    /// A data-only description of an optional machine feature.
    ///
    /// This decouples the *data* (name + price) from the *behaviour* (the decorator).
    /// Instances can be created from any source at runtime - a hardcoded list, a JSON
    /// file, a database, a configuration service, etc. - without recompiling the
    /// decorator or machine code.
    ///
    /// The price is authored in the base currency (GBP); it is converted to a country's
    /// local currency at quote time. Country availability determines which export markets
    /// the feature may be sold into.
    /// </summary>
    /// <param name="Key">Stable identifier used to select the feature (e.g. "quad-laser").</param>
    /// <param name="Name">Human readable display name.</param>
    /// <param name="Cost">Current price of the feature, in the base currency (GBP).</param>
    /// <param name="AvailableCountries">
    /// ISO country codes the feature may be sold in. An empty/null set means the feature is
    /// available in every country (a global feature).
    /// </param>
    public record FeatureDefinition(
        string Key,
        string Name,
        decimal Cost,
        IReadOnlySet<string>? AvailableCountries = null)
    {
        /// <summary>True when this feature can be sold in the given <paramref name="country"/>.</summary>
        public bool IsAvailableIn(Country country)
        {
            ArgumentNullException.ThrowIfNull(country);

            // No restrictions means the feature is available everywhere.
            return AvailableCountries is null
                || AvailableCountries.Count == 0
                || AvailableCountries.Contains(country.Code);
        }
    }
}
