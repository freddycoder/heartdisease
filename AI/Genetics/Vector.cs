using Genetic;
using System;
using System.Collections.Generic;
using System.Text;

namespace AI.Genetics
{
    public class Vector : Population<Factor>
    {
        public Vector(int populationSize) : base(populationSize)
        {
        }
    }
}
