using System;
using System.Windows;
using Aptacode.TaskPlex.Engine.Tasks.Transformations.Interpolation;
using Aptacode.TaskPlex.Interpolation.Easers;
using Aptacode.TaskPlex.WPF.Tasks.Transformation.Interpolator;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation
{
    public sealed class PointTransformation<TClass> : InterpolatedTransformation<TClass, Point> where TClass : class
    {
        public PointTransformation(TClass target,
            string property,
            TimeSpan duration,
            EaserFunction easerFunction = null,
            bool useStartValue = true,
            params Point[] values) : base(target,
            property,
            duration,
            new PointInterpolator(),
            easerFunction, useStartValue, values)
        {
        }
    }
}