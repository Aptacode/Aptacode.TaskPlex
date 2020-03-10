using System;
using Aptacode.TaskPlex.Interpolation.Easers;
using Aptacode.TaskPlex.Interpolation.Linear;

namespace Aptacode.TaskPlex.Engine.Tasks.Transformations.Interpolation
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