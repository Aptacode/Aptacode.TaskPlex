using System;
using System.Windows;
using Aptacode.TaskPlex.Interpolators.Easers;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolation;
using Aptacode.TaskPlex.WPF.Tasks.Transformation.Interpolator;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation
{
    public sealed class PointTransformation<TClass> : InterpolatedTransformation<TClass, Point> where TClass : class
    {
        public PointTransformation(TClass target,
            string property,
            TimeSpan duration,
            EaserFunction easerFunction = null,
            params Point[] values) : base(target,
            property,
            duration,
            new PointInterpolator(),
            easerFunction, values)
        {
        }
    }
}