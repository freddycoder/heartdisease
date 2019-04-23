using AI.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AI
{
    public class Agent<TDataModel> : IAgent<TDataModel> where TDataModel : IDataModel<int>
    {
        private Matrix _weights;
        private readonly int _vectorLenght;
        private int maxValue;
        public double learningRate { get; set; } = 0.1;

        static readonly Random _randomEngine = new Random();

        public Agent()
        {
            _vectorLenght = typeof(TDataModel).GetProperties().Where(p => p.Name != "Target").Count();

            _weights = new Matrix(1, _vectorLenght);

            maxValue = 100;

            GenerateNewValue(maxValue);
        }

        public Agent(int nbLayer)
        {
            Debug.Assert(nbLayer > 0, "You must have at least one layer");

            _vectorLenght = typeof(TDataModel).GetProperties().Where(p => p.Name != "Target").Count();

            _weights = new Matrix(nbLayer, _vectorLenght);

            maxValue = 100;

            GenerateNewValue(maxValue);
        }

        public Agent(Agent<TDataModel> agent)
        {
            _vectorLenght = agent._vectorLenght;

            _weights = agent._weights;

            maxValue = agent.maxValue;
        }

        public void Fit(List<TDataModel> datas)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                var error = 2 * (MakePrediction(datas[i]) - (2 * datas[i].Target - 1));

                var m = _randomEngine.Next(_weights.LinesCount);
                var n = _randomEngine.Next(_weights.ColumnsCount);

                _weights[m][n] -= 2 * learningRate * error;

                var newError = 2 * (MakePrediction(datas[i]) - (2 * datas[i].Target - 1));

                if (Math.Abs(newError) > Math.Abs(error))
                {
                    _weights[m][n] += 2 * learningRate * error;
                }
            }
        }

        public double MakePrediction(TDataModel data)
        {
            Matrix f = data.GetFeatures();

            for (int layer = 0; layer < _weights.LinesCount; layer++)
            {
                Matrix t = new Matrix(f.LinesCount, f.ColumnsCount);

                for (int feature = 0; feature < _weights.ColumnsCount; feature++)
                {
                    t[0][feature] = _weights[layer][feature] * f[0][feature];
                }

                f = t;
            }

            return (2 / (1 + Math.Pow(Math.E, -f.Sum()))) - 1;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Agent weights\t");
            sb.Append(Environment.NewLine);

            sb.Append(_weights.ToString());

            return sb.ToString();
        }

        private void GenerateNewValue(int max)
        {
            for (int i = 0; i < _weights.LinesCount; i++)
            {
                for (int j = 0; j < _weights.ColumnsCount; j++)
                {
                    var negatif = _randomEngine.Next() % 2 == 1 ? 1 : -1;
                    _weights[i][j] = negatif * (_randomEngine.NextDouble() * _randomEngine.Next() % max);
                }
            }
        }

        public Matrix GetWeights()
        {
            return _weights;
        }
    }
}
