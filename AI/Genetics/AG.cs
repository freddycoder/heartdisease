using AI.Reinforcement;
using Genetic;
using QLearning;
using System;

namespace AI.Genetics
{
    public class AG : GeneticAlgorithm<Factor>, IEnvironement<State, Reward, Reinforcement.Action>
    {
        public AG(int populationSize, double rankSpaceRatio, double mutationRatio, double selectionRatio, double uniformityRatio, IgenerateRandomIndividual<Factor> randomIndividualFunction) : base(populationSize, rankSpaceRatio, mutationRatio, selectionRatio, uniformityRatio, randomIndividualFunction)
        {
            
        }

        public State State
        {
            get
            {
                return new State { MutationRatio = getMutationRatio(), RankSpaceRatio = getRankSpaceRatio(), SelectionRatio = getSelectionRatio(), UniformityRatio = getUniformityRatio() };
            }
        }

        public Reward RecieveAction(Reinforcement.Action action)
        {
            setMutationRatio(action.NewState.MutationRatio);
            setRankSpaceRatio(action.NewState.RankSpaceRatio);
            setSelectionRatio(action.NewState.SelectionRatio);
            setUniformityRatio(action.NewState.UniformityRatio);

            startForGenerationCount(action.NbGeneration);

            return null;
        }

        public override string ToString()
        {
            return $"AG Setings : {getMutationRatio()} {getRankSpaceRatio()} {getGenerationCount()} AG pop : {getCurrentPopulation().ToString()}";
        }

        public dynamic[] GetPopVector()
        {
            var pop = new dynamic[getCurrentPopulation().size()];

            int i = 0;
            foreach (var indiv in getCurrentPopulation())
            {
                pop[i++] = indiv.value;
            }

            return pop;
        }
    }
}
