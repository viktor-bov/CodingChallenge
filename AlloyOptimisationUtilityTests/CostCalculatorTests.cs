using AlloyOptimisationUtility.Models;
using AlloyOptimisationUtility.Services;

namespace AlloyOptimisationUtilityTests
{
    [Trait("Category", "Unit")]
    public class CostCalculatorTests
    {
        private readonly ICostCalculator _calculator = new CostCalculator(Currency.Gbp);

        [Fact]
        public void Currency_IsRepresentedSeparately()
        {
            Assert.Equal("GBP", _calculator.Currency.Code);
            Assert.Equal("£", _calculator.Currency.Symbol);
        }

        [Fact]
        public void Calculate_IsSumOfAllElementContributions_IncludingBase()
        {
            // Cr 15, Co 10, Nb 1, Mo 2 => Ni balance 72.
            var composition = NickelTestData.Create(15.0, 10.0, 1.0, 2.0);

            double expected =
                14.0 * 15.0 / 100d +   // Cr
                80.5 * 10.0 / 100d +   // Co
                42.5 * 1.0 / 100d +    // Nb
                16.0 * 2.0 / 100d +    // Mo
                8.9 * 72.0 / 100d;     // Ni (base) still contributes

            Assert.Equal(expected, _calculator.Calculate(composition), 9);
        }

        [Fact]
        public void Calculate_PureBaseElement_UsesBaseCostOnly()
        {
            var composition = NickelTestData.Create(0, 0, 0, 0);

            // 100% Ni at £8.9/kg.
            Assert.Equal(8.9, _calculator.Calculate(composition), 9);
        }
    }
}
