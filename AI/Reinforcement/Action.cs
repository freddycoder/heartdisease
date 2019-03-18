using QLearning;
using System;
using System.Collections.Generic;
using System.Text;

namespace AI.Reinforcement
{
    public class Action : IAction
    {
        public State NewState { get; set; }
        public int NbGeneration { get; set; }
    }
}
