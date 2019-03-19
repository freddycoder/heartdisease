using System.Collections.Generic;
using Xunit;
using Stats;

namespace StatsTest
{
    public class Test
    {
        [Fact]
        public void ModeEasy()
        {
            var dist = new ContinueVariableList<int>(new List<dynamic>
            {
                1, 1, 1, 5, 5, 4, 4, 4, 4, 4, 4, 9, 34, 34, 1, 0, -100, 3
            });

            Assert.Equal(4, dist.Mode());
        }

        [Fact]
        public void Mean()
        {
            var dist = new ContinueVariableList<int>(new List<dynamic>
            {
                2, 2, 5, 9
            });

            Assert.Equal(4.5, dist.Mean());
        }

        [Fact]
        public void Median()
        {
            var dist = new ContinueVariableList<int>(new List<dynamic>
            {
                2, 3, 5, 9, -1, -2, 100
            });

            Assert.Equal(3, dist.Mediane());
        }

        [Fact]
        public void ContinueVariableOfContinueVariable()
        {
            var dist = new ContinueVariableList<ContinueVariableList<double>>(
                new List<dynamic> { new List<dynamic> { 0.1, 0.3 }, new List<dynamic> { 0.5, 0.7 } });

            Assert.Equal(0.1, dist[0][0]);
            Assert.Equal(0.3, dist[0][1]);
            Assert.Equal(0.5, dist[1][0]);
            Assert.Equal(0.7, dist[1][1]);
        }
    }
}
