using System;
using System.Numerics;
using Aptacode.Interpolatr;
using Aptacode.Interpolatr.Linear;

namespace Aptacode.TaskPlex.Stories.Transformations.Interpolation
{
    public sealed class Vec2Transformation<TClass> : InterpolatedTransformation<TClass, Vector2> where TClass : class
    {
        public Vec2Transformation(TClass target,
            string property,
            TimeSpan duration,
            EaserFunction easerFunction = null,
            bool useStartValue = true,
            params Vector2[] values) : base(target,
            property,
            duration,
            new Vec2LinearInterpolator(),
            easerFunction, useStartValue, values)
        {
        }
    }
}