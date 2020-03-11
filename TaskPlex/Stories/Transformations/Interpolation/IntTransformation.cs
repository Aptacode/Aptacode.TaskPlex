using System;
using Aptacode.Interpolatr;
using Aptacode.Interpolatr.Linear;

namespace Aptacode.TaskPlex.Stories.Transformations.Interpolation
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
            new IntInterpolator(),
            easerFunction,
            useStartValue,
            values)
        {
        }
    }
}