using System;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator
{
    public class DoubleInterpolator : Interpolator<double>
    {
        public DoubleInterpolator(double startValue, double endValue, TimeSpan duration, TimeSpan interval) : base(
            startValue, endValue, duration, interval)
        {
        }

        protected override double Add(double a, double b)
        {
            return a + b;
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