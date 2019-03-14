using Genetic;
using System;
using System.Collections.Generic;
using System.Text;

namespace AI.Genetics
{
    public class Factor : Individual<Factor>
    {
        public int value;

        public Factor(int value)
        {
            this.value = value;
        }

        public override void calculateFitness()
        {
            throw new NotImplementedException();
        }

        public override List<Factor> crossover(Individual<Factor> otherParent)
        {
            throw new NotImplementedException();
        }

        public override void mutate()
        {
            throw new NotImplementedException();
        }
    }
}
