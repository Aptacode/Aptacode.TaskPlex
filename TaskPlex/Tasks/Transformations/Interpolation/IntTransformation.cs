using System;
using Aptacode.TaskPlex.Interpolators;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformations.Interpolation
{
    public sealed class IntTransformation<TClass> : InterpolatedTransformation<TClass, int> where TClass : class
    {
        public IntTransformation(TClass target,
            string property,
            TimeSpan duration,
            EaserFunction easerFunction = null, params int[] values) : base(target,
            property,
            duration,
            new IntInterpolator(),
            easerFunction,
            values)
        {
        }
    }
}