using System;
using System.Drawing;
using Aptacode.Interpolatr;
using Aptacode.Interpolatr.Linear;

namespace Aptacode.TaskPlex.Stories.Transformations.Interpolation
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