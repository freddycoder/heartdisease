using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AI
{
    public class Agent<TDataModel> where TDataModel : IData
    {
        private readonly double[] _factors;
        private readonly double[] _bestFactor;
        private readonly double[] _bestFactorLifeTime;
        private Dictionary<int, int[]> NbTimeValueEncounter { get; set; }
        private readonly int _vectorLenght;
        private readonly Random _randomEngine;
        private int nbCorrectPredictionBestScore;
        private int maxValue;

        public Agent()
        {
            _randomEngine = new Random();

            _vectorLenght = typeof(TDataModel).GetProperties().Length - 1;

            _factors = new double[_vectorLenght];

            _bestFactor = new double[_vectorLenght];

            _bestFactorLifeTime = new double[_vectorLenght];

            maxValue = 150;

            GenerateNewValue(maxValue);

            NbTimeValueEncounter = new Dictionary<int, int[]>();
        }

        public Agent(Agent<TDataModel> agent)
        {
            _randomEngine = new Random();

            _vectorLenght = agent._vectorLenght;

            _factors = new double[_vectorLenght];

            _bestFactor = new double[_vectorLenght];

            _bestFactorLifeTime = new double[_vectorLenght];

            for (int i = 0; i < _vectorLenght; i++)
            {
                _factors[i] = agent._bestFactorLifeTime[i];
            }

            NbTimeValueEncounter = new Dictionary<int, int[]>();
        }

        public Agent(double[] _vector)
        {
            Debug.Assert(typeof(TDataModel).GetProperties().Length - 1 == _vector.Length);

            _vectorLenght = _vector.Length;

            _factors = _vector;
        }

        public void TrainOnDatas(List<TDataModel> datas)
        {
            int nbCorrectPrediction = 0;

            foreach (var data in datas)
            {
                bool prediction = MakePrediction(data) > 0;

                if ((data.Target == 0 && prediction == false) || (data.Target == 1 && prediction == true))
                {
                    nbCorrectPrediction++;

                    for (int i = 0; i < _vectorLenght; i++)
                    {
                        if (NbTimeValueEncounter.ContainsKey((int)_factors[i]))
                        {
                            NbTimeValueEncounter[(int)_factors[i]][i]++;
                        }
                        else
                        {
                            NbTimeValueEncounter.Add((int)_factors[i], new int[_vectorLenght]);
                            NbTimeValueEncounter[(int)_factors[i]][i] = 1;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < _vectorLenght; i++)
                    {
                        int negatif = _randomEngine.Next() % 2 == 1 ? 1 : -1;

                        int nbTimeSee = NbTimeValueEncounter.ContainsKey((int)_factors[i]) ?
                            NbTimeValueEncounter[(int)_factors[i]][i] : 0;

                        _factors[i] += negatif * _randomEngine.Next(maxValue % Math.Max(1, nbTimeSee));
                    }
                }
            }

            if (nbCorrectPrediction < datas.Count / 3)
            {
                GenerateNewValue(maxValue);
                Forgot();                
            }
            else if (nbCorrectPrediction < nbCorrectPredictionBestScore)
            {
                for (int i = 0; i < _vectorLenght; i++)
                {
                    _factors[i] = _bestFactor[i];
                }
            }
            else
            {
                nbCorrectPredictionBestScore = nbCorrectPrediction;
                for (int i = 0; i < _vectorLenght; i++)
                {
                    _bestFactor[i] = _factors[i];
                    _bestFactorLifeTime[i] = _bestFactor[i];
                }
            }
        }

        public double MakePrediction(TDataModel data)
        {
            double value = Calculate(data);

            return (2 / (1 + Math.Pow(Math.E, -value))) - 1;
        }

        private double Calculate(TDataModel data)
        {
            int i = 0;
            double value = 0;
            foreach (var prop in data.GetType().GetProperties())
            {
                if (prop.Name != "Target")
                {
                    value += double.Parse(prop.GetValue(data).ToString()) / _factors[i] == 0 ? 1 : _factors[i];
                    i++;
                }
            }

            return value;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Agent\t");
            sb.Append(Environment.NewLine);

            for (int i = 0; i < _factors.Length; i++)
            {
                sb.Append($"{i}\t | {_factors[i]}\t");
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        private void GenerateNewValue(int max)
        {
            for (int i = 0; i < _vectorLenght; i++)
            {
                int negatif = _randomEngine.Next() % 2 == 1 ? 1 : -1;

                _factors[i] = negatif * _randomEngine.NextDouble() * (_randomEngine.Next() % max);
                _bestFactor[i] = _factors[i];
            }
        }

        private void Forgot()
        {
            foreach (var pair in NbTimeValueEncounter)
            {
                for (int i = 0; i < _vectorLenght; i++)
                {
                    if (pair.Value[i] > 1)
                    {
                        pair.Value[i] = (int)(pair.Value[i] / 1.5);
                    }
                }
            }
        }
    }
}
