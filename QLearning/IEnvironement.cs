using System;
using System.Collections.Generic;
using System.Text;

namespace QLearning
{
    public interface IEnvironement<TState, TReward, TAction> where TState : IState where TReward : IReward where TAction : IAction
    {
        TState State { get; }
        TReward RecieveAction(TAction action);
    }
}
