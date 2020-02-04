using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;
using System;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator
{
    public class DoubleInterpolator : Interpolator<double>
    {
        /// <summary>
        /// Calculates values between the start and end double returning values at the given interval until the
        /// specified     duration
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <param name="interval"></param>
        public DoubleInterpolator(double startValue, double endValue, TimeSpan duration, RefreshRate refreshRate) : base(startValue,
                                                                                                                   endValue,
                                                                                                                   duration,
                                                                                                                   refreshRate)
        { }

        public DoubleInterpolator(double startValue, double endValue, TimeSpan duration, RefreshRate refreshRate, Easer easer) : base(startValue,
                                                                                                                                endValue,
                                                                                                                                duration,
                                                                                                                                refreshRate,
                                                                                                                                easer)
        { }

        protected override double Add(double a, double b) => a + b;

        protected override double Divide(double a, int incrementCount)
        {
            if(incrementCount <= 1)
            {
                return a;
            }

            return a / incrementCount;
        }

        protected override double Subtract(double a, double b) => a - b;
    }
}