using AI.Genetics;
using Functions;
using Heart;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using System;

namespace Genetic.Test
{
    public class MainTest
    {
        [Fact]
        public void GetBestIndividual()
        {
            var algo = new GeneticAlgorithm<Factor>(new FactorGenerator(45));

            var myIndividu = algo.getCurrentPopulation().getIndividualAtIndex(5);

            myIndividu.setFitnessScore(1);

            myIndividu.value = -150;

            Assert.Equal(-150, algo.getBestIndividual().value);
        }

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

            var ag = new AG(
                datas.GetType().GetProperties().Length - 1,
                0.5,
                0.5,
                0.5,
                0.5,
                new FactorGenerator(100)
            );

            var function = new VectorMultiplication();

            var nbGoodPredriction = 0;
            var nbTurn = 0;
            var idata = 0;
            while (nbGoodPredriction != datas.Count)
            {
                dynamic[] pop = ag.GetPopVector();

                if (function.F(pop, datas[idata].ToArray()) > 0 == datas[idata] == 1)
                {
                    nbGoodPredriction++;
                }

                idata++;
                nbTurn++;

                foreach (var indiv in ag.getCurrentPopulation())
                {
                    indiv.nbExperiences += 2;
                    indiv.nbGoodExperiences += nbGoodPredriction;
                }

                if (nbGoodPredriction != datas.Count)
                {
                    ag.startForGenerationCount(1);
                }

                if (idata == 2)
                {
                    idata = 0;
                }
            }

            for (int i = 0; i < 2; i++)
            {
                Assert.True(function.F(ag.GetPopVector(), datas[i].ToArray()) > 0 == datas[i] == 1);
            }
        }

        [Fact]
        public void ShouldResolveOneTrueAndOneLessColoumnFalse()
        {
            var datas = new List<HealthInfoSmallModel>
            {
                new HealthInfoSmallModel
                {
                    Age = 63,
                    Cp = 3,
                    Target = 1
                },
                new HealthInfoSmallModel
                {
                    Age = 67,
                    Cp = 0,
                    Target = 0
                }
            };

            var ag = new AG(
                datas.GetType().GetProperties().Length - 1,
                0.5,
                0.2,
                1,
                0.5,
                new FactorGenerator(3)
            );

            var function = new VectorMultiplication();

            var nbGoodPredriction = 0;
            var nbTurn = 0;
            while (nbGoodPredriction != datas.Count)
            {
                dynamic[] pop = ag.GetPopVector();

                foreach (var data in datas)
                {
                    if (Convert.ToBoolean(function.F(pop, data.ToArray().SkipLast(1).ToArray()) > 0) == (data.Target == 1))
                    {
                        nbGoodPredriction++;
                    }
                }

                nbTurn++;

                foreach (var indiv in ag.getCurrentPopulation())
                {
                    indiv.nbExperiences += 2;
                    indiv.nbGoodExperiences += nbGoodPredriction;
                }

                if (nbGoodPredriction != datas.Count)
                {
                    ag.startForGenerationCount(1);
                }

                nbGoodPredriction = 0;
            }

            for (int i = 0; i < 2; i++)
            {
                Assert.True(function.F(ag.GetPopVector(), datas[i].ToArray()) > 0 == datas[i] == 1);
            }
        }
    }
}
