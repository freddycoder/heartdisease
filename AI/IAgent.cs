using System;
using System.Collections.Generic;
using System.Text;

namespace AI
{
    public interface IAgent<TDataModel> where TDataModel : IDataModel<int>
    {
        void Fit(List<TDataModel> datas);
        double MakePrediction(TDataModel data);
    }
}
