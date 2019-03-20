using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stats
{
    public class ContinueVariableList<T> : Component, IEnumerable<T>, IComparable where T : IComparable
    {
        public IList<dynamic> _variables;

        public ContinueVariableList(List<dynamic> variables)
        {
            _variables = variables;
        }

        public dynamic this[int key]
        {
            get
            {
                return _variables[key];
            }
            
            set => _variables[key] = value;
        }

        public void Add(T elem)
        {
            _variables.Add(elem);
        }

        public int NbClasses()
        {
            return (int)(1 + Math.Log(_variables.Count, 2));
        }

        public dynamic Etendu()
        {
            return _variables.Max() - _variables.Min();
        }

        public dynamic Amplitude()
        {
            return Etendu() / NbClasses();
        }

        public dynamic Mode()
        {
            var elemCount = new Dictionary<T, int>();

            foreach (var elem in _variables)
            {
                if (!elemCount.ContainsKey(elem))
                {
                    elemCount.Add(elem, 1);
                }
                else
                {
                    elemCount[elem]++;
                }
            }

            var first = elemCount.First();
            T mode = first.Key;
            int count = first.Value;
            foreach (var kvp in elemCount)
            {
                if (count < kvp.Value)
                {
                    mode = kvp.Key;
                    count = kvp.Value;
                }
            }

            return mode;
        }

        public int Count => _variables.Count;

        public dynamic Mean()
        {
            return _variables.Sum(v => v) / (double)_variables.Count;
        }

        public dynamic Mediane()
        {
            var temps = new List<T>();

            foreach (var elem in this)
            {
                temps.Add(elem);
            }

            temps.Sort();

            return temps[_variables.Count / 2];
        }

        public List<ContinueVariableList<T>> Quantiles(int nbQuantile)
        {
            ((List<T>)_variables).Sort();

            var quantiles = new List<ContinueVariableList<T>>(nbQuantile);

            int j = 0;
            for (int i = 0; i < nbQuantile; i++)
            {
                if (i != 0 && (i % (Count / nbQuantile) == 0))
                {
                    j++;
                }

                quantiles[j].Add(_variables[i]);
            }

            return quantiles;
        }

        public dynamic StdDeviation()
        {
            dynamic specialSum = default;
            dynamic mean = Mean();
            foreach (var elem in _variables)
            {
                specialSum += Math.Pow(elem - mean, 2);
            }

            return Math.Sqrt(specialSum / (Count - 1));
        }

        public dynamic ZScore(dynamic elem)
        {
            return (elem - Mean()) / StdDeviation();
        }

        public dynamic VariationCoef()
        {
            return StdDeviation() / Mean();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var temps = new List<T>(Count);

            foreach (var item in _variables)
            {
                temps.Add((T)item);
            }

            return temps.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _variables.GetEnumerator();
        }

        public int CompareTo(object obj)
        {
            if (obj.GetType().GetInterfaces().Contains(typeof(IEnumerable)))
            {
                IEnumerable enumerable = (IEnumerable)obj;

                int count = 0;
                foreach (var elem in enumerable)
                {
                    count++;
                }

                return Count.CompareTo(count);
            }
            else
            {
                throw new InvalidCastException("Cannot comapre those type, sorry. See the git repo to add this comparaison"); ;
            }
        }

        public override dynamic[] GetArray()
        {
            return _variables.ToArray();
        }
    }
}
