using Genetic;
using System;
using System.Collections.Generic;
using System.Text;

namespace AI.Genetics
{
    public class FactorGenerator : IgenerateRandomIndividual<Factor>
    {
        static Random randEngine = new Random();
        public Factor generateRandomIndividual()
        {
            return new Factor(randEngine.Next());
        }
    }
}
