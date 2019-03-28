using System;
using System.Collections.Generic;
using System.Text;

namespace Heart
{
    public class DecisionNode
    {
        private Dictionary<string, Dictionary<int, int[]>> _tree;

        public DecisionNode()
        {
            _tree = new Dictionary<string, Dictionary<int, int[]>>();
        }

        public void Score(string property, int category)
        {
            var matrix = _tree[property][category];

            int uniformity = 0;

            for (int i = 0; i < matrix.Length; i++)
            {

            }
        }

        public void Add(string property, int category, int target)
        {
            if (_tree.ContainsKey(property))
            {
                if (_tree[property].ContainsKey(category))
                {
                    _tree[property][category][target]++;
                }
                else
                {
                    _tree[property].Add(category, new int[2]);
                    _tree[property][category][target]++;
                }
            }
            else
            {
                _tree.Add(property, new Dictionary<int, int[]>());
                _tree[property].Add(category, new int[2]);
                _tree[property][category][target]++;
            }
        }
    }
}
