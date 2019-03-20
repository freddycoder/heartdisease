using System;
using Xunit;

namespace Heart.Test
{
    public class UnitTest1
    {
        [Fact]
        public void ToArray()
        {
            var dataSet = new HealthInfo
            {
                Age = 1,
                Sex = 2,
                Cp = 3,
                Trestbps = 4,
                Chol = 5,
                Fbs = 6,
                Restecg = 7,
                Thalach = 8,
                Exang = 9,
                Oldpeak = 10,
                Slope = 11,
                Ca = 12,
                Thal = 13,
                Target = 14
            };

            var vector = dataSet.ToArray();

            for (int i = 1; i <= vector.Length; i++)
            {
                Assert.Equal(i, vector[i - 1]);
            }
        }
    }
}
