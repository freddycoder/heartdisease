using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Genetic
{
    public class Population<T> : IEnumerable<T> where T : Individual<T>
    {

        private List<T> population;
        private int populationCapacity;
        private readonly Random random = new Random();

        public Population(int populationSize)
        {
            this.populationCapacity = populationSize;
            this.population = new List<T>(populationSize);
        }

        public void addIndividual(T individual)
        {
            if (this.population.Count < this.populationCapacity)
            {
                this.population.Add(individual);
            }
        }

        public void addGroupOfIndividual(List<T> individuals)
        {
            for (int i = 0; i < individuals.Count; i++)
            {
                if (this.population.Count < this.populationCapacity)
                {
                    this.population.Add(individuals[i]);
                }
                else
                {
                    return;
                }
            }
        }

        public void removeIndividual(T individual)
        {
            this.population.Remove(individual);
        }

        public void removeIndividualAt(int index)
        {
            this.population.RemoveAt(index);
        }

        public int size()
        {
            return this.population.Count;
        }

        public T getRandomIndividual()
        {
            return this.population[(this.random.Next(this.population.Count))];
        }

        public T getIndividualAtIndex(int index)
        {
            return this.population[index];
        }

        public int getPopulationCapacity() { return this.populationCapacity; }

        public bool contains(T other)
        {
            foreach (T individual in this)
            {
                if (individual.Equals(other))
                {
                    return true;
                }
            }
            return false;
        }

        public void sortPopulationByFitness()
        {
            this.population.Sort();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return population.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            foreach (var elem in this)
            {
                sb.Append(elem.ToString());
                sb.Append(",");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            return sb.ToString();
        }
    }
}

