using AI.Genetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QLearning;
using AI.Reinforcement;
using Stats;
using Functions;

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

        private IFunction _predictor;

        public Agent(IEnumerable<DataModel> dataModels)
        {
            _randEngine = new Random();

            _dataModelFields = typeof(DataModel).GetProperties()
                .Where(p => p.Name != "Target").ToArray();

            _dataSet = dataModels;

            G = new AG(
                typeof(DataModel).GetProperties().Length - 1,
                0.5,
                0.5,
                0.5,
                0.5,
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

            _predictor = new VectorMultiplication();
        }

        public bool Predict(DataModel data)
        {
            dynamic[] vector = new dynamic[G.getCurrentPopulation().size()];

            for (int i = 0; i < G.getCurrentPopulation().size(); i++)
            {
                vector[i] = G.getCurrentPopulation().getIndividualAtIndex(i).value;
            }

            return _predictor.F(vector, data.ToArray().SkipLast(1).ToArray()) > 0;
        }

        public void Train(Func<Agent<DataModel>, bool> whenToStop)
        {
            while (!whenToStop.Invoke(this))
            {
                lastTrainGoodResult = 0;
                foreach (var d in _dataSet)
                {
                    if (Predict(d) == d.Target > 0)
                    {
                        lastTrainGoodResult++;
                    }
                }

                ReceiveReward(new Reward(lastTrainGoodResult, _dataSet.Count()));

                var newAction = Action(G.State);

                G.RecieveAction(newAction);
            }
        }

        private int lastTrainGoodResult;
        private int lastTrainNbObj;

        public double TrainingScore()
        {
            return (double)lastTrainGoodResult / lastTrainNbObj;
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
