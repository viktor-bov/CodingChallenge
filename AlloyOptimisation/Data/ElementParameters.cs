namespace AlloyOptimisation.Data
{
    /// <summary>
    /// Material-physics parameters for a single alloying element, as read from configuration.
    /// This is a plain data-transfer object shaped for deserialization; it deliberately holds
    /// no behaviour so the same shape can be sourced from a hardcoded JSON string today and
    /// from Azure App Configuration or a database in the future.
    /// </summary>
    public sealed class ElementParameters
    {
        /// <summary>
        /// Creep coefficient (alpha) in m^2/s per atomic percent. Null for base elements
        /// (e.g. nickel) that have no creep contribution.
        /// </summary>
        public double? CreepCoefficient { get; init; }

        /// <summary>Minimum concentration (atomic percent).</summary>
        public double Minimum { get; init; }

        /// <summary>Maximum concentration (atomic percent).</summary>
        public double Maximum { get; init; }

        /// <summary>Concentration step size (atomic percent).</summary>
        public double Step { get; init; }
    }
}
