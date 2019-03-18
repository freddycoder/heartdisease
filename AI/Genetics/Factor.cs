using Genetic;
using System;
using System.Collections.Generic;

namespace AI.Genetics
{
    public class Factor : Individual<Factor>
    {
        private static Random _randEngine = new Random();

        public int value;
        public int nbGoodExperiences;
        public int nbExperiences;

        public Factor(int value)
        {
            this.value = value;
            this.nbGoodExperiences = 0;
            this.nbExperiences = 0;
        }

        public override void calculateFitness()
        {
            this.setFitnessScore(Math.Pow(nbExperiences != 0 ? nbGoodExperiences / nbExperiences : 0, 2));
        }

        public override List<Factor> crossover(Individual<Factor> otherParent)
        {
            var factors = new List<Factor>();

            int firstChild = this.value ^ otherParent.getT().value;

            factors.Add(new Factor(firstChild));

            int otherChild = 0;

            for (int i = 0; i < 32; i++)
            {
                otherChild |= firstChild & (1 << i);
            }

            factors.Add(new Factor(otherChild));

            return factors;
        }

        public override void mutate()
        {
            int pos = 1 << (_randEngine.Next() % 12);

            value |= pos;
        }

        public override Factor getT()
        {
            return this;
        }

        public override string ToString()
        {
            return $"Value: {value} GoodExp: {nbExperiences} NbExp: {nbExperiences} Fitness: {getFitnessScore()}";
        }
    }
}
