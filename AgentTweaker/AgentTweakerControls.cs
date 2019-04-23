using System;
using System.Collections.Generic;
using System.Text;

namespace AgentTweaker
{
    public interface AgentTweakerControls
    {
        int NbLayer { get; set; }
        double LearningRate { get; set; }
    }
}
