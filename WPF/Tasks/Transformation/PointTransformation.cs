using System;
using System.Windows;
using Aptacode.TaskPlex.Interpolators.Easers;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolation;
using Aptacode.TaskPlex.WPF.Tasks.Transformation.Interpolator;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation
{
    public sealed class PointTransformation<TClass> : InterpolatedTransformation<TClass, Point> where TClass : class
    {
        private PointTransformation(TClass target,
            string property,
            Func<Point> endValue,
            TimeSpan duration,
            EaserFunction easerFunction = null) : base(target,
            property,
            endValue,
            duration,
            new PointInterpolator(),
            easerFunction)
        {
        }

        /// <summary>
        ///     Transform an int property on the target object to the value returned by the given Func at intervals
        ///     specified by     the step duration up to the task duration
        /// </summary>
        public static PointTransformation<T> Create<T>(T target, string property, Point endValue, TimeSpan duration,
            EaserFunction easerFunction = null) where T : class
        {
            try
            {
                return new PointTransformation<T>(target, property, () => endValue, duration, easerFunction);
            }
            catch
            {
                return null;
            }
        }
    }
}