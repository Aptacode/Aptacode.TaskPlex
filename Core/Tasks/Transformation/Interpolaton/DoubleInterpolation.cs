using System;
using System.Reflection;

namespace Aptacode.Core.Tasks.Transformations.Interpolation
{
    public class DoubleInterpolation : Interpolation<double>
    {
        public DoubleInterpolation(object target, PropertyInfo property, Func<double> destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
        }

        public DoubleInterpolation(object target, PropertyInfo property, double destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
        }
        protected override double Add(double a, double b)
        {
            return a + b;
        }

        protected override double Divide(double a, int b)
        {
            return a / b;
        }

        protected override double Subtract(double a, double b)
        {
            return a - b;
        }
    }
}
