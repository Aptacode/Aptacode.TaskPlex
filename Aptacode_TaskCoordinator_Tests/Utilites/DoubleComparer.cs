using System;
using System.Collections.Generic;
using System.Text;

namespace Aptacode_TaskCoordinator.Tests.Utilites
{
    public class DoubleComparer : IEqualityComparer<double>
    {
        private readonly double epsilon;
        public DoubleComparer()
        {
            epsilon = 0.001;
        }
        public bool Equals(double x, double y)
        {
            return Math.Abs(x - y) < this.epsilon;
        }

        public int GetHashCode(double obj)
        {
            return obj.GetHashCode();
        }
    }
}
