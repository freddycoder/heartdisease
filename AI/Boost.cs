using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI
{
    public class Boost<TDataModel> : IAgent<TDataModel> where TDataModel : IDataModel<int>
    {
        private readonly List<IAgent<TDataModel>> _agents;
        private readonly List<double> _pounds;

        public Boost()
        {
            _agents = new List<IAgent<TDataModel>>();
            _pounds = new List<double>();
        }

        public void Add(IAgent<TDataModel> agent)
        {
            _agents.Add(agent);
            _pounds.Add(0.0);
        }

        public void Set(int index, IAgent<TDataModel> agent)
        {
            _agents[index] = agent;
        }

        public IAgent<TDataModel> Get(int index)
        {
            return _agents[index];
        }

        public int Count { get => _agents.Count; }

        public double MakePrediction(TDataModel data)
        {
            double value = 0.0;

            for (int i = 0; i < Count; i++)
            {
                value += _agents[i].MakePrediction(data) * _pounds[i];
            }

            return (2 / (1 + Math.Pow(Math.E, -value))) - 1;
        }

        public void Fit(List<TDataModel> datas, int nbEpoch)
        {
            for (int i = 0; i < nbEpoch; i++)
            {
                Fit(datas);
            }
        }

        public void Fit(List<TDataModel> datas)
        {
            var wrongAnswers = new double[Count];

            for (int i = 0; i < Count; i++)
            {
                _agents[i].Fit(datas);

                foreach (var data in datas)
                {
                    if ((_agents[i].MakePrediction(data) > 0) != (data.Target > 0))
                    {
                        wrongAnswers[i]++;
                    }
                }
            }

            for (int i = 0; i < Count; i++)
            {
                _pounds[i] = wrongAnswers[i] / datas.Count;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var agent in _agents)
            {
                sb.Append(agent.ToString());
            }

            return sb.ToString();
        }
    }
}
