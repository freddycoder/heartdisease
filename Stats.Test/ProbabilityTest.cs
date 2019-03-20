using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Stats.Test
{
    public class ProbabilityTest
    {
        [Fact]
        public void PigerUnAs()
        {
            var cardStack = new List<dynamic>(54);

            for (int i = 0; i < 54; i++)
            {
                cardStack.Add(i + 1);
            }

            var vector = new ContinueVariableList<int>(cardStack);

            var probEngine = new Probability(vector);

            Assert.Equal(1 / 13, probEngine.Classic(n => n <= 4));
        }
    }
}
