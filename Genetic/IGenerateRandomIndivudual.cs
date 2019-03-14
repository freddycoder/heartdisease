using System;

namespace Genetic
{
    public interface IgenerateRandomIndividual<TIndividu> where TIndividu : Individual<TIndividu>
    {
        TIndividu generateRandomIndividual();
    }
}
