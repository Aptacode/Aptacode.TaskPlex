using System;
using Aptacode.TaskPlex.Interpolators;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformations.Interpolation
{
    public sealed class DoubleTransformation<TClass> : InterpolatedTransformation<TClass, double> where TClass : class
    {
        /// <summary>
        ///     Transform a double property on the target object to the value returned by the given Func<>at intervals
        ///     specified by the step duration up to the task duration
        /// </summary>
        /// <summary>
        private DoubleTransformation(TClass target,
            string property,
            Func<double> endValue,
            int stepCount, EaserFunction easerFunction = null) : base(target,
            property,
            endValue,
            stepCount,
            new DoubleInterpolator(), easerFunction)
        {
        }

        public static DoubleTransformation<T> Create<T>(T target, string property, double endValue, int duration,
            EaserFunction easerFunction = null) where T : class
        {
            try
            {
                return new DoubleTransformation<T>(target, property, () => endValue, duration, easerFunction);
            }
            catch
            {
                return null;
            }
        }
    }
}