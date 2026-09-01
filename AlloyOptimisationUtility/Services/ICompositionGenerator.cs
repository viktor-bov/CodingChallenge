using AlloyOptimisationUtility.Models;

namespace AlloyOptimisationUtility.Services
{
    public interface ICompositionGenerator
    {
        /// <summary>
        /// Lazily yields every valid composition (base balance &gt;= 0) in the search space.
        /// </summary>
        IEnumerable<AlloyComposition> Generate(AlloySystem system);
    }
}
