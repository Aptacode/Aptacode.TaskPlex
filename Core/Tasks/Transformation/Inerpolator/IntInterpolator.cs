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

        protected override Comparison Compare(int a, int b)
        {
            if (a > b)
                return Comparison.Greater;
            else if (b > a)
                return Comparison.Less;
            else
                return Comparison.Equal;
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
