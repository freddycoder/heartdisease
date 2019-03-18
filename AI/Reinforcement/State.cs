using QLearning;

namespace AI.Reinforcement
{
    public class State : IState
    {
        public double RankSpaceRatio { get; set; }
        public double MutationRatio { get; set; }
        public double SelectionRatio { get; set; }
        public double UniformityRatio { get; set; }
    }
}
