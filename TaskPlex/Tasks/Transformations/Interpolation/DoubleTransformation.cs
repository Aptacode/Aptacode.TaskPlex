using System;
using Aptacode.TaskPlex.Interpolators;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformations.Interpolation
{
    public sealed class DoubleTransformation<TClass> : InterpolatedTransformation<TClass, double> where TClass : class
    {
        /// <summary>
        ///     Transform a double property on the target object to the value returned by the given Func<>at intervals
        ///     specified by the step duration up to the task duration
        /// </summary>
        /// <summary>
        public DoubleTransformation(TClass target,
            string property,
            TimeSpan stepCount,
            EaserFunction easerFunction = null,
            params double[] values) : base(target,
            property,
            stepCount,
            new DoubleInterpolator(), easerFunction, values)
        {
        }
    }
}