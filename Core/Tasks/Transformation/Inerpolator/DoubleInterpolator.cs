using System;
using System.Reflection;

namespace Aptacode.Core.Tasks.Transformations.Interpolation
{
    public class DoubleInterpolator : Interpolator<double>
    {
        public DoubleInterpolator(double startValue, double endValue, TimeSpan duration, TimeSpan interval) : base(startValue, endValue, duration, interval)
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
