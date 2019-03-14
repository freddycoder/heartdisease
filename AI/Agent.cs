using AI.Genetics;
using Genetic;
using System;
using System.Collections.Generic;
using System.Text;

namespace AI
{
    public class Agent<DataModel> where DataModel : IData
    {
        private GeneticAlgorithm<Factor> GeneticAlgorithm { get; set; }

        public Agent()
        {
            GeneticAlgorithm = new GeneticAlgorithm<Factor>(
                typeof(DataModel).GetProperties().Length - 1,
                0.25,
                0.01,
                0.80,
                0.02,
                new FactorGenerator());
        }

        public void AddData(List<DataModel> datas)
        {
            GeneticAlgorithm.startForGenerationCount(10);
        }

        public bool MakePrediction(DataModel data)
        {
            var result = Calculate(data);

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private double Calculate(DataModel data)
        {
            double val = 0;

            for (int i = GeneticAlgorithm.getPopulationSize() - 1; i >= 0; i--)
            {
                val += Math.Pow(GeneticAlgorithm.getCurrentPopulation().getIndividualAtIndex(i).value, i);
            }

            return val;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Index\t | Factor\t\t");
            sb.Append(Environment.NewLine);

            for (int i = 0; i < GeneticAlgorithm.getCurrentPopulation().size(); i++)
            {
                sb.Append($"{i}\t | {GeneticAlgorithm.getCurrentPopulation().getIndividualAtIndex(i)}\t");
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
