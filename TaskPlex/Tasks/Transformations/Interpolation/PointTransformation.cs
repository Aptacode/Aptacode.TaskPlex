using System;
using System.Drawing;
using Aptacode.TaskPlex.Interpolators;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformations.Interpolation
{
    public sealed class PointTransformation<TClass> : InterpolatedTransformation<TClass, Point> where TClass : class
    {
        internal PointTransformation(TClass target,
            string property,
            Func<Point> endValue,
            int duration, EaserFunction easerFunction = null) : base(target,
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
        public static PointTransformation<T> Create<T>(T target, string property, Point endValue, int duration,
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