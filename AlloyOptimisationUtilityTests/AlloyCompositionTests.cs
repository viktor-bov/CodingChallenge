using AlloyOptimisationUtility.Models;

namespace AlloyOptimisationUtilityTests
{
    [Trait("Category", "Unit")]
    public class AlloyCompositionTests
    {
        [Fact]
        public void TryCreate_CalculatesBaseElementAsBalance()
        {
            var composition = NickelTestData.Create(15.0, 10.0, 1.0, 2.0);

            Assert.Equal(72.0, composition.BasePercentage, 9);
            Assert.Equal(72.0, composition.Percentages[NickelTestData.Nickel], 9);
        }

        [Fact]
        public void TryCreate_TotalCompositionEquals100()
        {
            var composition = NickelTestData.Create(20.0, 5.0, 0.5, 3.0);

            Assert.Equal(100.0, composition.TotalPercentage, 9);
        }

        [Fact]
        public void TryCreate_NegativeBalance_IsRejected()
        {
            // Alloying elements sum to 130 > 100, so the base balance would be negative.
            var result = AlloyComposition.TryCreate(
                NickelTestData.CreateSystem(),
                new[] { 60.0, 40.0, 10.0, 20.0 });

            Assert.Null(result);
        }

        [Fact]
        public void TryCreate_Exactly100_IsAccepted_WithZeroBase()
        {
            var result = AlloyComposition.TryCreate(
                NickelTestData.CreateSystem(),
                new[] { 50.0, 30.0, 10.0, 10.0 });

            Assert.NotNull(result);
            Assert.Equal(0.0, result!.BasePercentage, 9);
        }
    }
}
