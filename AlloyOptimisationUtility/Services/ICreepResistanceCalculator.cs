using AlloyOptimisationUtility.Models;

namespace AlloyOptimisationUtility.Services
{
    public interface ICreepResistanceCalculator
    {
        /// <summary>Creep resistance in m^2/s.</summary>
        double Calculate(AlloyComposition composition);
    }
}
