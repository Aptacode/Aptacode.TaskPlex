using System;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator
{
    public class DoubleInterpolator : Interpolator<double>
    {
        /// <summary>
        ///     Calculates values between the start and end double returning values at the given interval until the specified
        ///     duration
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <param name="interval"></param>
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
            if (incrementCount <= 1)
            {
                return a;
            }

            return a / incrementCount;
        }

        protected override double Subtract(double a, double b)
        {
            return a - b;
        }
    }
}