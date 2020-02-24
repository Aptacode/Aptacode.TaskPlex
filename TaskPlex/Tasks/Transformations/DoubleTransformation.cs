using System;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolators;

namespace Aptacode.TaskPlex.Tasks.Transformations
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
            TimeSpan taskDuration,
            RefreshRate refreshRate = RefreshRate.Normal) : base(target,
            property,
            endValue,
            taskDuration,
            new DoubleInterpolator(),
            refreshRate)
        {
        }

        public static DoubleTransformation<T> Create<T>(T target, string property, double endValue, TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            try
            {
                return new DoubleTransformation<T>(target, property, () => endValue, duration, refreshRate);
            }
            catch
            {
                return null;
            }
        }
    }
}