using System;
using Aptacode.TaskPlex.Interpolators;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformations.Interpolation
{
    public sealed class IntTransformation<TClass> : InterpolatedTransformation<TClass, int> where TClass : class
    {
        private IntTransformation(TClass target,
            string property,
            Func<int> endValue,
            TimeSpan duration,
            EaserFunction easerFunction = null) : base(target,
            property,
            endValue,
            duration,
            new IntInterpolator(),
            easerFunction)
        {
        }

        /// <summary>
        ///     Transform an int property on the target object to the value returned by the given Func at intervals
        ///     specified by     the step duration up to the task duration
        /// </summary>
        public static IntTransformation<T> Create<T>(T target, string property, int endValue, TimeSpan duration,
            EaserFunction easerFunction = null) where T : class
        {
            try
            {
                return new IntTransformation<T>(target, property, () => endValue, duration, easerFunction);
            }
            catch
            {
                return null;
            }
        }
    }
}