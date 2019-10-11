using System;
using System.Reflection;

namespace Aptacode.Core.Tasks.Transformations.Interpolation
{
    public class DoubleInterpolator : Interpolator<double>
    {
        public DoubleInterpolator(object target, string property, Func<double> destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
        }

        public DoubleInterpolator(object target, string property, double destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
        }

        protected override double Add(double a, double b)
        {
            return a + b;
        }

        protected override Comparison Compare(double a, double b)
        {
            if (a > b)
                return Comparison.Greater;
            else if (b > a)
                return Comparison.Less;
            else
                return Comparison.Equal;
        }

        protected override double Divide(double a, int incrementCount)
        {
            return a / incrementCount;
        }

        protected override double Subtract(double a, double b)
        {
            return a - b;
        }
    }
}
