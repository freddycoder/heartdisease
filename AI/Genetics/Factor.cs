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
            if (nbExperiences == 0) return;

            double score = nbGoodExperiences / nbExperiences;

            this.setFitnessScore(Math.Pow(score, 2) - 0.25);
        }

        public override List<Factor> crossover(Individual<Factor> otherParent)
        {
            var factors = new List<Factor>();

            int firstChild = this.value ^ otherParent.getT().value;

            factors.Add(new Factor(firstChild));

            int otherChild = 0;

            for (int i = 0; i < 32; i++)
            {
                if (Math.Pow(2, i) < Math.Max(value, otherParent.getT().value) && (value & (1 << i)) == ((otherParent.getT().value) & (1 << i)))
                    otherChild |= (1 << i);
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
            return $"Value: {value} GoodExp: {nbGoodExperiences} NbExp: {nbExperiences} Fitness: {getFitnessScore()}";
        }
    }
}
