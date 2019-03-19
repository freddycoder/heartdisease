using QLearning;
using System;
using System.Collections.Generic;
using System.Text;

namespace AI.Reinforcement
{
    public class Reward : IReward
    {
        private double reward;
        private int nbGoodAnswer;
        private int nbTotalQuestion;

        public Reward(int goodAnswer, int totalQuestion)
        {
            this.reward = (double)goodAnswer / totalQuestion;
            this.nbGoodAnswer = goodAnswer;
            this.nbTotalQuestion = totalQuestion;
        }

        public Reward(double reward)
        {
            this.reward = reward;
        }

        public double GetReward()
        {
            return reward;
        }

        public int GetNbGoodAnswer()
        {
            return nbGoodAnswer;
        }

        public int GetNbTotalQuestion()
        {
            return nbTotalQuestion;
        }
    }
}
