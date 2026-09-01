using AlloyOptimisationUtility;

namespace AlloyOptimisationUtilityTests
{
    [Trait("Category", "Unit")]
    public class UnitTest1
    {
        [Theory]
        
        [InlineData()]
        public void UnitOfWork_InitialCondition_ExpectedResult()
        {
            //Arrange
            Class1 addNumbers = new Class1();

            //Act
            var actualResult = addNumbers.Add(1, 2);

            //Assert
            Assert.Equal(3, actualResult);
        }
    }
}
