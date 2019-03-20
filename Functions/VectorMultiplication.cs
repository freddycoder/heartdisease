using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Functions
{
    public class VectorMultiplication : IFunction
    {
        public dynamic F(dynamic[] a, dynamic[] x)
        {
            Debug.Assert(a.Length == x.Length);

            dynamic sum = 0.0;

            for (int i = 0; i < a.Length; i++)
            {
                sum += a[i] * x[i];
            }

            return sum;
        }
    }
}
