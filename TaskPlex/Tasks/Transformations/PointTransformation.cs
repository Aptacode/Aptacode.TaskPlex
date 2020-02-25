using System;
using System.Drawing;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolators;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolators.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformations
{
    public sealed class PointTransformation<TClass> : InterpolatedTransformation<TClass, Point> where TClass : class
    {
        private PointTransformation(TClass target,
            string property,
            Func<Point> endValue,
            TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal, EaserFunction easerFunction = null) : base(target,
            property,
            endValue,
            duration,
            new PointInterpolator(),
            refreshRate, easerFunction)
        {
        }

        /// <summary>
        ///     Transform an int property on the target object to the value returned by the given Func at intervals
        ///     specified by     the step duration up to the task duration
        /// </summary>
        public static PointTransformation<T> Create<T>(T target, string property, Point endValue, TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal, EaserFunction easerFunction = null) where T : class
        {
            try
            {
                return new PointTransformation<T>(target, property, () => endValue, duration, refreshRate, easerFunction);
            }
            catch
            {
                return null;
            }
        }
    }
}