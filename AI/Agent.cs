using AI.Genetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QLearning;
using AI.Reinforcement;

namespace AI
{
    public class Agent<DataModel> : IAgent<Reinforcement.Action, State, Reward> where DataModel : IData
    {
        private IEnumerable<DataModel> _dataSet;

        private PropertyInfo[] _dataModelFields;

        private AG G;

        private readonly double [][] Q;

        private long time;

        private Random _randEngine;

        public Agent(IEnumerable<DataModel> dataModels)
        {
            _randEngine = new Random();

            _dataModelFields = typeof(DataModel).GetProperties()
                .Where(p => p.Name != "Target").ToArray();

            _dataSet = dataModels;

            G = new AG(
                typeof(DataModel).GetProperties().Length - 1,
                0.25,
                0.01,
                0.80,
                0.02,
                new FactorGenerator(150));

            Q = new double[100][];

            for (int i = 0; i < 100; i++)
            {
                Q[i] = new double[100];

                for (int j = 0; j < 100; j++)
                {
                    Q[i][j] = _randEngine.NextDouble();
                }
            }
        }

        public bool Predict(DataModel data)
        {
            double answer = 0;

            for (int i = 0; i < _dataModelFields.Length; i++)
            {
                double dataValue = 0;
                if (_dataModelFields[i].PropertyType.Equals(typeof(double)))
                {
                    dataValue = (double)_dataModelFields[i].GetValue(data);
                }
                else
                {
                    dataValue = (int)_dataModelFields[i].GetValue(data);
                }

                answer += G.getCurrentPopulation().getIndividualAtIndex(i).value * Math.Pow(dataValue, i);
            }

            return answer > 0;
        }

        public void Train()
        {
            ReceiveReward(
                G.RecieveAction(
                    Action(G.State)));
        }

        public Reinforcement.Action Action(State state)
        {
            int minIndex = 50;
            double minValue = double.MaxValue;
            int maxIndex = 50;
            double maxValue = double.MinValue;

            double uniMin = 0;
            double uniMax = 0;

            for (int i = 0; i < Q.Length; i++)
            {
                for (int j = 0; j < Q[i].Length; j++)
                {
                    if (minValue > Q[i][j])
                    {
                        minValue = Q[i][j];
                        minIndex = i;
                        uniMin = j;
                    }
                    if (maxValue < Q[i][j])
                    {
                        maxValue = Q[i][j];
                        maxIndex = i;
                        uniMax = j;
                    }
                }
            }

            var newState = G.State;

            var r = _randEngine.Next(10000);

            double choice = 0.0;
            double choiceUni = 0.0;
            if (r < 10000 + time++ * -0.5)
            {
                choice = minIndex / 100.0;
                choiceUni = uniMin / 100.0;
            }
            else
            {
                choice = maxIndex / 100.0;
                choiceUni = uniMax / 100.0;
            }

            newState.MutationRatio = choice;
            newState.UniformityRatio = choiceUni;

            return new Reinforcement.Action { NewState = newState, NbGeneration = 1 };
        }

        public void ReceiveReward(Reward reward)
        {
            for (int i = 0; i < G.getCurrentPopulation().size(); i++)
            {
                G.getCurrentPopulation().getIndividualAtIndex(i)
                    .nbExperiences += reward.GetNbTotalQuestion();

                G.getCurrentPopulation().getIndividualAtIndex(i)
                    .nbGoodExperiences += reward.GetNbGoodAnswer();
            }

            Q[(int)G.State.MutationRatio * 100][(int)G.State.UniformityRatio * 100] += reward.GetReward();
        }

        public override string ToString()
        {
            return this.G.ToString();
        }
    }
}
