using System;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolators;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolators.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformations
{
    public sealed class IntTransformation<TClass> : InterpolatedTransformation<TClass, int> where TClass : class
    {
        private IntTransformation(TClass target,
            string property,
            Func<int> endValue,
            TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal, EaserFunction easerFunction = null) : base(target,
            property,
            endValue,
            duration,
            new IntInterpolator(),
            refreshRate, easerFunction)
        {
        }

        /// <summary>
        ///     Transform an int property on the target object to the value returned by the given Func at intervals
        ///     specified by     the step duration up to the task duration
        /// </summary>
        public static IntTransformation<T> Create<T>(T target, string property, int endValue, TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal, EaserFunction easerFunction = null) where T : class
        {
            try
            {
                return new IntTransformation<T>(target, property, () => endValue, duration, refreshRate, easerFunction);
            }
            catch
            {
                return null;
            }
        }
    }
}