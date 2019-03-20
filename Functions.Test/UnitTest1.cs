using Xunit;

namespace Functions.Test
{
    public class UnitTest1
    {
        [Fact]
        public void VectorMultiplicationTest()
        {
            var multiplicator = new VectorMultiplication();

            Assert.Equal(3, multiplicator.F(new dynamic[]{ 1, 3, -5 }, new dynamic[]{ 4, -2, -1 }));
        }
    }
}
