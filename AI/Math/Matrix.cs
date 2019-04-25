using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AI.Mathematics
{
    public class Matrix : IEnumerable<double>
    {
        private readonly double[][] _array;
        public static Matrix Identity(int m, int n)
        {
            var M = new Matrix(m, n);

            for (int i = 0; i < M.LinesCount; i++)
            {
                for (int j = 0; j < M.ColumnsCount; j++)
                {
                    if (i == j) M[i][j] = 1.0;
                }
            }

            return M;
        }
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

        public Matrix(double[] array)
        {
            _array = new double[1][];

            _array[0] = array;
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

        public void SetEachValueTo(double val)
        {
            for (int i = 0; i < LinesCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    _array[i][j] = 0;
                }
            }
        }

        public IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < LinesCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    yield return _array[i][j];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < LinesCount; i++)
            {
                yield return _array[i].GetEnumerator();
            }
        }

        public Matrix Inverse()
        {
            var m = new Matrix(ColumnsCount, LinesCount);

            for (int i = 0; i < LinesCount; i++)
            {
                for (int j = 0; j < LinesCount; j++)
                {
                    m[j][i] = this[i][j];
                }
            }

            return m;
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
                    for (int k = 0; k < Y.LinesCount; k++)
                    {
                        result[i][j] += X[i][k] * Y[k][j];
                    }
                }
            }

            return result;
        }

        public static Matrix operator - (Matrix a, double b)
        {
            for (int i = 0; i < a.LinesCount; i++)
            {
                for (int j = 0; j < a.ColumnsCount; j++)
                {
                    a[i][j] -= b;
                }
            }

            return a;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < LinesCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    sb.Append(this[i][j] + " ");
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
