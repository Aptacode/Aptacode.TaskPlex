using System;
using System.Numerics;
using Aptacode.TaskPlex.Interpolation.Easers;
using Aptacode.TaskPlex.Interpolation.Linear;

namespace Aptacode.TaskPlex.Engine.Tasks.Transformations.Interpolation
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
            new Vec2Interpolator(),
            easerFunction, useStartValue, values)
        {
        }
    }
}