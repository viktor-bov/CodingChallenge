namespace DesignPatternsAndPrinciples
{
    /// <summary>
    /// A data-only description of an optional machine feature.
    ///
    /// This decouples the *data* (name + price) from the *behaviour* (the decorator).
    /// Instances can be created from any source at runtime - a hardcoded list, a JSON
    /// file, a database, a configuration service, etc. - without recompiling the
    /// decorator or machine code.
    /// </summary>
    /// <param name="Key">Stable identifier used to select the feature (e.g. "quad-laser").</param>
    /// <param name="Name">Human readable display name.</param>
    /// <param name="Cost">Current price of the feature.</param>
    public record FeatureDefinition(string Key, string Name, decimal Cost);
}
