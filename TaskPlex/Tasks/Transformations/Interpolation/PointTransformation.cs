using System;
using System.Drawing;
using Aptacode.TaskPlex.Interpolators.Easers;
using Aptacode.TaskPlex.Interpolators.Linear;

namespace Aptacode.TaskPlex.Tasks.Transformations.Interpolation
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
            new PointLinearInterpolator(),
            easerFunction, useStartValue, values)
        {
        }
    }
}