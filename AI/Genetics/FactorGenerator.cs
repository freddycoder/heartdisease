using Genetic;
using System;
using System.Collections.Generic;
using System.Text;

namespace AI.Genetics
{
    public class FactorGenerator : IgenerateRandomIndividual<Factor>
    {
        static Random randEngine = new Random();

        public int max;

        public FactorGenerator(int max)
        {
            this.max = max;
        }

        public Factor generateRandomIndividual()
        {
            var negatif = randEngine.Next() % 2 == 1 ? 1 : -1;

            return new Factor(negatif * randEngine.Next() % max);
        }
    }
}
