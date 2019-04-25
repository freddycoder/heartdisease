using System;
using System.Collections.Generic;
using System.Text;

namespace AI.CalculateFunctions
{
    public class VectorDivisionForEachProp<TDataModel> : ICalculateFunction<TDataModel> where TDataModel : IDataModel<int>
    {
        public double Calculate(TDataModel model, double[] factors)
        {
            int i = 0;
            double value = 0;
            foreach (var prop in model.GetType().GetProperties())
            {
                if (prop.Name != "Target")
                {
                    value += double.Parse(prop.GetValue(model).ToString()) / factors[i] == 0 ? 1 : factors[i];
                    i++;
                }
            }

            return value;
        }
    }
}
