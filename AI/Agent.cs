using AI.Genetics;
using Genetic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace AI
{
    public class Agent<DataModel> where DataModel : IData
    {
        private GeneticAlgorithm<Factor> G { get; set; }

        private PropertyInfo[] _dataModelFields;

        public Agent()
        {
            _dataModelFields = typeof(DataModel).GetProperties()
                .Where(p => p.Name != "Target").ToArray();

            G = new GeneticAlgorithm<Factor>(
                typeof(DataModel).GetProperties().Length - 1,
                0.25,
                0.01,
                0.80,
                0.02,
                new FactorGenerator(150));
        }

        public void Train(List<DataModel> datas)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                var goolPred = false;

                if (MakePrediction(datas[i])) goolPred = true;

                foreach (var x in G.getCurrentPopulation())
                {
                    if (goolPred) x.nbGoodExperiences++;
                    x.nbExperiences++;
                }
            }

            G.startForGenerationCount(1);
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

            for (int i = G.getPopulationSize() - 1; i >= 0; i--)
            {
                double propValue = 0.0;
                if (_dataModelFields[i].PropertyType.Equals(typeof(double)))
                {
                    propValue = (double)_dataModelFields[i].GetValue(data);
                }
                else
                {
                    propValue = (int)_dataModelFields[i].GetValue(data);
                }

                val += G.getCurrentPopulation().getIndividualAtIndex(i).value * Math.Pow(propValue, i);
            }

            return val;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Index\t | Factor\t\t");
            sb.Append(Environment.NewLine);

            for (int i = 0; i < G.getCurrentPopulation().size(); i++)
            {
                sb.Append($"{i}\t | {G.getCurrentPopulation().getIndividualAtIndex(i).getT().value}\t");
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
