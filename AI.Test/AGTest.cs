using Xunit;
using Heart;
using System.Collections.Generic;

namespace AI.Test
{
    public class AGTest
    {
        [Fact]
        public void ShouldResolveOneTrueAndOneFalse()
        {
            var datas = new List<HealthInfo>
            {
                new HealthInfo
                {
                    Age = 63,
                    Sex = 1,
                    Cp = 3,
                    Trestbps = 145,
                    Chol = 233,
                    Fbs = 1,
                    Restecg = 0,
                    Thalach = 150,
                    Exang = 0,
                    Oldpeak = 2.3,
                    Slope = 0,
                    Ca = 0,
                    Thal = 1,
                    Target = 1
                },
                new HealthInfo
                {
                    Age = 67,
                    Sex = 1,
                    Cp = 0,
                    Trestbps = 160,
                    Chol = 286,
                    Fbs = 0,
                    Restecg = 0,
                    Thalach = 108,
                    Exang = 1,
                    Oldpeak = 1.5,
                    Slope = 1,
                    Ca = 3,
                    Thal = 2,
                    Target = 0
                }
            };

            var algo = new Agent<HealthInfo>(datas);

            algo.Train(a => a.TrainingScore() == 1);

            Assert.True(algo.Predict(datas.Find(h => h.Target == 1)));
            Assert.False(algo.Predict(datas.Find(h => h.Target == 0)));
        }
    }
}
