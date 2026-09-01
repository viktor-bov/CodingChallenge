using AlloyOptimisationUtility.Models;

namespace AlloyOptimisationUtility.Services
{
    public interface ICostCalculator
    {
        Currency Currency { get; }

        /// <summary>Total cost per kilogram in <see cref="Currency"/> units.</summary>
        double Calculate(AlloyComposition composition);
    }
}
