using AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heart
{
    public class HealthInfoSmallModel : IData
    {
        public int Age { get; set; }
        public int Cp { get; set; }
        public int Target { get; set; }

        public dynamic[] ToArray()
        {
            var properties = GetType().GetProperties();
            dynamic[] vector = new dynamic[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                vector[i] = properties[i].GetValue(this);
            }

            return vector;
        }
    }
}
