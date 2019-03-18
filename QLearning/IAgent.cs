using System;
using System.Collections.Generic;
using System.Text;

namespace QLearning
{
    public interface IAgent<TAction, TState, TReward> where TAction : IAction where TState : IState where TReward : IReward
    {
        TAction Action(TState state);
        void ReceiveReward(TReward reward);
    }
}
