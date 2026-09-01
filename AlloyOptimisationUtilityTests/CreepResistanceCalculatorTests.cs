using AlloyOptimisationUtility.Services;

namespace AlloyOptimisationUtilityTests
{
    [Trait("Category", "Unit")]
    public class CreepResistanceCalculatorTests
    {
        private readonly ICreepResistanceCalculator _calculator = new CreepResistanceCalculator();

        [Theory]
        // Validation examples from Table 3. Ni is the balance and contributes 0.
        [InlineData(15.0, 10.0, 1.00, 2.00, 1.226E18)]
        [InlineData(20.0, 0.0, 0.00, 1.50, 5.519E17)]
        [InlineData(22.0, 25.0, 1.50, 6.00, 2.820E18)]
        public void Calculate_MatchesExpectedCreepResistance(double cr, double co, double nb, double mo, double expected)
        {
            var composition = NickelTestData.Create(cr, co, nb, mo);

            double actual = _calculator.Calculate(composition);

            // Table values are rounded to 4 significant figures, so allow ~0.1% tolerance.
            Assert.Equal(expected, actual, expected * 1E-3);
        }

        [Fact]
        public void Calculate_BaseElement_ContributesZero()
        {
            // Pure base element: all alloying percentages zero, creep should be zero.
            var composition = NickelTestData.Create(0, 0, 0, 0);

            Assert.Equal(0d, _calculator.Calculate(composition), 6);
        }
    }
}
