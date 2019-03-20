using System;
using System.Collections.Generic;
using System.Text;

namespace Stats
{
    public class Probability : Decorator
    {
        public Probability(Component component)
        {
            _component = component;
        }

        public double Classic(Func<dynamic, bool> func)
        {
            int count = 0;

            foreach (var num in GetArray())
            {
                if (func.Invoke(num))
                {
                    count++;
                }
            }

            return count / GetArray().Length;
        }

        public override dynamic[] GetArray()
        {
            return _component.GetArray();
        }
    }
}
