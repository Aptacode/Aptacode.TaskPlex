using System;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator
{
    public class IntInterpolator : Interpolator<int>
    {
        /// <summary>
        ///     Calculates values between the start and end int returning values at the given interval until the specified duration
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <param name="interval"></param>
        public IntInterpolator(int startValue, int endValue, TimeSpan duration, TimeSpan interval) : this(startValue,
            endValue, duration, interval, new LinearEaser())
        {
        }

        public IntInterpolator(int startValue, int endValue, TimeSpan duration, TimeSpan interval, Easer easer) : base(
            startValue,
            endValue, duration, interval, easer)
        {
        }

        protected override int Add(int a, int b)
        {
            return a + b;
        }

        protected override int Divide(int a, int incrementCount)
        {
            if (incrementCount <= 1)
            {
                return a;
            }

            return a / incrementCount;
        }

        protected override int Subtract(int a, int b)
        {
            return a - b;
        }
    }
}