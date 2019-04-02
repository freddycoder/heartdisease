using System;

namespace AI.Mathematics
{
    public class Matrix
    {
        private readonly double[][] _array;

        public Matrix(int m, int n)
        {
            if (m <= 0 || n <= 0)
            {
                throw new InvalidOperationException($"Cannot build such matrix ({m},{n})");
            }

            _array = new double[m][];

            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = new double[n];
            }
        }

        public int LinesCount { get => _array.Length; }
        public int ColumnsCount { get =>  _array[0].Length; }

        public double[] this[int i]
        {
            get
            {
                return _array[i];
            }
            set
            {
                _array[i] = value;
            }
        }

        public static Matrix operator * (Matrix X, Matrix Y)
        {
            if (X.ColumnsCount != Y.LinesCount)
            {
                throw new InvalidOperationException("The number of column of the first matrix must math the number of line of the second matrix");
            }

            var result = new Matrix(X.LinesCount, Y.ColumnsCount);

            for (int i = 0; i < result.LinesCount; i++)
            {
                for (int j = 0; j < result.ColumnsCount; j++)
                {
                    for (int k = 0; k < X.LinesCount; k++)
                    {
                        result[i][j] += X[i][k] * Y[k][j];
                    }
                }
            }

            return result;
        }
    }
}
