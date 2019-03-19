using AI.Genetics;
using Xunit;

namespace Genetic.Test
{
    public class GetInfoTest
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
    }
}
