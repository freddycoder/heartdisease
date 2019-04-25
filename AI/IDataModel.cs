using AI.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AI
{
    public interface IDataModel<TargetType>
    {
        TargetType Target { get; set; }
        string Header();
        Matrix GetFeatures();
    }
}
