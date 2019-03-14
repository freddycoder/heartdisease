using System;
using System.Collections.Generic;
using System.Text;

namespace Genetic
{
    public abstract class Individual<T> : IComparable<Individual<T>>
    {
        private double fitnessScore;

        public abstract List<T> crossover(Individual<T> otherParent);

        public abstract void mutate();

        public abstract void calculateFitness();

        public double getFitnessScore()
        {
            return this.fitnessScore;
        }

        public void setFitnessScore(double score)
        {
            this.fitnessScore = score;
        }

        public int CompareTo(Individual<T> obj)
        {
            return getFitnessScore().CompareTo(obj.getFitnessScore());
        }
    }

}
