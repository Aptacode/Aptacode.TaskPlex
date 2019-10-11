using System;

namespace Aptacode.Core.Tasks.Transformations.Interpolation
{
    public class IntInterpolator : Interpolator<int>
    {
        public IntInterpolator(int startValue, int endValue, TimeSpan duration, TimeSpan interval) : base(startValue, endValue, duration, interval)
        {

        }

        protected override int Add(int a, int b)
        {
            return a + b;
        }

        protected override int Divide(int a, int incrementCount)
        {
            return a / incrementCount;
        }

        protected override int Subtract(int a, int b)
        {
            return a - b;
        }
    }
}
