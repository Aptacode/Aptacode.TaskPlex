using System;
using Aptacode.TaskPlex.Interpolation.Easers;
using Aptacode.TaskPlex.Interpolation.Linear;

namespace Aptacode.TaskPlex.Engine.Tasks.Transformations.Interpolation
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
            bool useStartValue = false,
            params double[] values) : base(target,
            property,
            stepCount,
            new DoubleLinearInterpolator(), easerFunction, useStartValue, values)
        {
        }
    }
}