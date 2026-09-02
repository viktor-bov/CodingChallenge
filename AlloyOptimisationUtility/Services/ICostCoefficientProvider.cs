using AlloyOptimisationUtility.Models;

namespace AlloyOptimisationUtility.Services
{
    /// <summary>
    /// Supplies the market cost coefficient (price per kilogram) for an element.
    /// <para>
    /// Cost coefficients change with market conditions, so they are resolved through this
    /// abstraction rather than being compiled into the model. Implementations can read from
    /// an in-memory table, a configuration file, or a database without any change to the
    /// calculation code that depends on this interface.
    /// </para>
    /// </summary>
    public interface ICostCoefficientProvider
    {
        /// <summary>
        /// Returns the current cost coefficient for <paramref name="element"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is <c>null</c>.</exception>
        /// <exception cref="CostCoefficientNotFoundException">
        /// No cost coefficient is available for the element.
        /// </exception>
        double GetCostCoefficient(Element element);
    }
}
