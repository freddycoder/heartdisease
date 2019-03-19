using AI.Genetics;
using Xunit;

namespace AI.Test
{
    public class FitnessScoreTest
    {
        [Fact]
        public void FitnessScoreBasic()
        {
            var factor = new Factor(45);

            factor.nbExperiences = 2;
            factor.nbGoodExperiences = 1;

            factor.calculateFitness();

            Assert.Equal(-0.25, factor.getFitnessScore());
        }

        [Fact]
        public void NoExperience()
        {
            var factor = new Factor(45);

            factor.calculateFitness();

            Assert.Equal(0, factor.getFitnessScore());
        }

        [Fact]
        public void LookLikeGoodFactor()
        {
            var factor = new Factor(45);

            factor.nbExperiences = 85;
            factor.nbGoodExperiences = 100;

            factor.calculateFitness();

            Assert.Equal(0.75, factor.getFitnessScore());
        }
    }
}
