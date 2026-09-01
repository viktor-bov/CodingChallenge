namespace AlloyOptimisationUtility.Models
{
    public sealed class ElementConstraint
    {
        public ElementConstraint(Element element, double minimum, double maximum, double step)
        {
            Element = element ?? throw new ArgumentNullException(nameof(element));

            if (minimum < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minimum), "Minimum concentration cannot be negative.");
            }

            if (maximum < minimum)
            {
                throw new ArgumentOutOfRangeException(nameof(maximum), "Maximum concentration cannot be less than the minimum.");
            }

            if (step <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(step), "Step size must be positive.");
            }

            Element = element;
            Minimum = minimum;
            Maximum = maximum;
            Step = step;
        }

        public Element Element { get; }

        public double Minimum { get; }

        public double Maximum { get; }

        public double Step { get; }

        public int StepCount => (int)Math.Round((Maximum - Minimum) / Step, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Returns the concentration for a given integer step index. Computing the value
        /// as <c>Minimum + Step * index</c> (rather than repeatedly adding the step)
        /// avoids accumulated floating-point error such as 0.1 + 0.1 + 0.1 != 0.3.
        /// </summary>
        public double ValueAt(int stepIndex)
        {
            if (stepIndex < 0 || stepIndex > StepCount)
            {
                throw new ArgumentOutOfRangeException(nameof(stepIndex));
            }

            return Minimum + (Step * stepIndex);
        }
    }
}
