using System;
using System.Drawing;
using Aptacode.TaskPlex.Interpolators;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformations.Interpolation
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