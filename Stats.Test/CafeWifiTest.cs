using System.Collections.Generic;
using Xunit;
using Stats;

namespace StatsTest
{
    public class CafeWifiTest
    {
        private readonly ContinueVariableList<int> distribution;

        public CafeWifiTest()
        {
            var minutesOnWifiByClient = new List<dynamic>
            {
                48, 35, 29, 44, 42, 52, 43, 38, 40, 47,
                30, 56, 32, 49, 40, 59, 37, 39, 40, 46,
                53, 37, 48, 45, 46, 42, 43, 35, 33, 51,
                26, 45, 41, 41, 34, 38, 43, 41, 38, 35
            };

            distribution = new ContinueVariableList<int>(minutesOnWifiByClient);
        }

        [Fact]
        public void Count()
        {
            Assert.Equal(40, distribution.Count);
        }

        [Fact]
        public void Etendue()
        {
            Assert.Equal(33, (double)distribution.Etendu());
        }

        [Fact]
        public void NbClasses()
        {
            Assert.Equal(6, distribution.NbClasses());
        }

        [Fact]
        public void Amplitude()
        {
            Assert.Equal(5, (int)distribution.Amplitude());
        }

        [Fact]
        public void Mode()
        {
            Assert.Equal(35, (int)distribution.Mode());
        }
    }
}
