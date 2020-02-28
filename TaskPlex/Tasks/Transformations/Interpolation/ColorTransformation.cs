using System;
using System.Drawing;
using Aptacode.TaskPlex.Interpolators;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformations.Interpolation
{
    public sealed class ColorTransformation<TClass> : InterpolatedTransformation<TClass, Color> where TClass : class
    {
        public ColorTransformation(TClass target,
            string property,
            TimeSpan duration, EaserFunction easerFunction = null, params Color[] values) : base(target,
            property,
            duration,
            new ColorInterpolator(), easerFunction, values)
        {
        }
    }
}