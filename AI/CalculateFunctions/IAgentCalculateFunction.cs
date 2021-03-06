﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AI.CalculateFunctions
{
    public interface ICalculateFunction<TDataModel> where TDataModel : IDataModel<int>
    {
        double Calculate(TDataModel model, double[] factors);
    }
}
