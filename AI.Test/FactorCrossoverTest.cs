using AI.Genetics;
using Xunit;

namespace AI.Test
{
    public class FactorCrossoverTest
    {
        [Fact]
        public void CrossoverBase()
        {
            var f1 = new Factor(0b0110);
            var f2 = new Factor(0b1010);

            var childrens = f1.crossover(f2);

            Assert.Equal(0b1100, childrens[0].value);
            Assert.Equal(0b0011, childrens[1].value);
        }
    }
}
