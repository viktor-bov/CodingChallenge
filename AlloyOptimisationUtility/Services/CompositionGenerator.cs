using AlloyOptimisationUtility.Models;

namespace AlloyOptimisationUtility.Services
{
    public sealed class CompositionGenerator : ICompositionGenerator
    {
        public IEnumerable<AlloyComposition> Generate(AlloySystem system)
        {
            ArgumentNullException.ThrowIfNull(system);

            int dimensionCount = system.AlloyElementsWithConstraints.Count;
            var indices = new int[dimensionCount];
            // Reused buffer to avoid allocating a new array for each candidate composition.
            var percentages = new double[dimensionCount];

            while (true)
            {
                double alloyingSum = 0d;
                for (int i = 0; i < dimensionCount; i++)
                {
                    double value = system.AlloyElementsWithConstraints[i].ValueAt(indices[i]);
                    percentages[i] = value;
                    alloyingSum += value;
                }

                // Only attempt to build a composition when the base balance is non-negative.
                if (alloyingSum <= 100d)
                {
                    var composition = AlloyComposition.TryCreate(system, percentages);
                    if (composition is not null)
                    {
                        yield return composition;
                    }
                }

                if (!Advance(indices, system.AlloyElementsWithConstraints))
                {
                    yield break;
                }
            }
        }

        /// <summary>
        /// Advances the odometer-style index array to the next combination.
        /// Returns false once every combination has been produced.
        /// </summary>
        private static bool Advance(int[] indices, IReadOnlyList<ElementConstraint> constraints)
        {
            for (int dimension = 0; dimension < indices.Length; dimension++)
            {
                if (indices[dimension] < constraints[dimension].StepCount)
                {
                    indices[dimension]++;
                    return true;
                }

                indices[dimension] = 0;
            }

            return false;
        }
    }
}
