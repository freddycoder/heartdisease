using AI.Genetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QLearning;
using AI.Reinforcement;
using Stats;

namespace AI
{
    public class Agent<DataModel> : IAgent<Reinforcement.Action, State, Reward> where DataModel : IData
    {
        private IEnumerable<DataModel> _dataSet;

        private PropertyInfo[] _dataModelFields;

        private AG G;

        private ContinueVariableList<ContinueVariableList<double>> Q;

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
                _randEngine.NextDouble(),
                _randEngine.NextDouble(),
                _randEngine.NextDouble(),
                _randEngine.NextDouble(),
                new FactorGenerator(150));

            var subQ = new List<dynamic>(100);

            for (int i = 0; i < 100; i++)
            {
                var doubles = new List<dynamic>(100);

                for (int j = 0; j < 100; j++)
                {
                    doubles.Add(0.0);
                }

                subQ.Add(doubles);
            }

            Q = new ContinueVariableList<ContinueVariableList<double>>(subQ);
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

            int rankMin = 0;
            int rankMax = 0;

            for (int i = 0; i < Q.Count; i++)
            {
                for (int j = 0; j < Q[i].Count; j++)
                {
                    if (minValue > Q[i][j])
                    {
                        minValue = Q[i][j];
                        minIndex = i;
                        rankMin = j;
                    }
                    if (maxValue < Q[i][j])
                    {
                        maxValue = Q[i][j];
                        maxIndex = i;
                        rankMax = j;
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
                choiceUni = rankMin / 100.0;
            }
            else
            {
                choice = maxIndex / 100.0;
                choiceUni = rankMax / 100.0;
            }

            newState.MutationRatio = choice + 0.01;
            newState.RankSpaceRatio = choiceUni + 0.01;

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

            Q[((int)(G.State.MutationRatio - 0.01)) * 100][((int)(G.State.RankSpaceRatio - 0.01)) * 100] += reward.GetReward();
        }

        public override string ToString()
        {
            return this.G.ToString();
        }
    }
}
