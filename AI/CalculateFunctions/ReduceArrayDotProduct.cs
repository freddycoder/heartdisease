using AI.Mathematics;
using System.Linq;

namespace AI.CalculateFunctions
{
    public class ReduceArrayDotProduct<TDataModel> : ICalculateFunction<TDataModel> where TDataModel : IDataModel<int>
    {
        public double Calculate(TDataModel model, double[] factors)
        {
            var data = typeof(TDataModel).GetProperties()
                .Single(p => p.PropertyType.Equals(typeof(double[])))
                .GetValue(model) as double[];

            var matrix = new Matrix(data.Length / factors.Length, factors.Length);

            for (int i = 0; i < matrix.LinesCount; i++)
            {
                for (int j = 0; j < matrix.ColumnsCount; j++)
                {
                    matrix[i][j] = data[i * matrix.ColumnsCount + j];
                }
            }

            var summator = Matrix.Identity(matrix.ColumnsCount, matrix.ColumnsCount);

            var reduit = matrix * summator;

            var factorMatrix = new Matrix(factors).Inverse();

            return (reduit * factorMatrix)[0][0];
        }
    }
}
