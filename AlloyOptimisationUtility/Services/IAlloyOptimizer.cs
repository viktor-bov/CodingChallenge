using AlloyOptimisationUtility.Models;

namespace AlloyOptimisationUtility.Services
{
    public interface IAlloyOptimizer
    {
        /// <summary>
        /// Finds the composition with the highest creep resistance whose cost does not
        /// exceed <paramref name="maximumCost"/>. Returns <c>null</c> when no valid
        /// composition satisfies the constraint.
        /// </summary>
        OptimisationResult? Optimise(AlloySystem system, double maximumCost);
    }
}
