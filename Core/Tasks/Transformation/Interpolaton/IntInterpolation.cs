using System;
using System.Reflection;

namespace Aptacode.Core.Tasks.Transformations.Interpolation
{
    public class IntInterpolation : Interpolation<int>
    {
        public IntInterpolation(object target, PropertyInfo property, Func<int> destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
        }

        public IntInterpolation(object target, PropertyInfo property, int destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
        }

        protected override int Add(int a, int b)
        {
            return a + b;
        }

        protected override int Divide(int a, int b)
        {
            return a / b;
        }

        protected override int Subtract(int a, int b)
        {
            return a - b;
        }
    }
}
