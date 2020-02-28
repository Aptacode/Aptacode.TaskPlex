using System;
using Aptacode.TaskPlex.Interpolators.Easers;
using Aptacode.TaskPlex.Interpolators.Linear;

namespace Aptacode.TaskPlex.Tasks.Transformations.Interpolation
{
    public sealed class IntTransformation<TClass> : InterpolatedTransformation<TClass, int> where TClass : class
    {
        public IntTransformation(TClass target,
            string property,
            TimeSpan duration,
            EaserFunction easerFunction = null,
            bool useStartValue = false,
            params int[] values) : base(target,
            property,
            duration,
            new IntLinearInterpolator(),
            easerFunction,
            useStartValue,
            values)
        {
        }
    }
}