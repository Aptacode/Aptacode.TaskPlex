using System;
using System.Drawing;
using Aptacode.TaskPlex.Interpolation.Easers;
using Aptacode.TaskPlex.Interpolation.Linear;

namespace Aptacode.TaskPlex.Engine.Tasks.Transformations.Interpolation
{
    public sealed class ColorTransformation<TClass> : InterpolatedTransformation<TClass, Color> where TClass : class
    {
        public ColorTransformation(TClass target,
            string property,
            TimeSpan duration,
            EaserFunction easerFunction = null,
            bool useStartValue = false,
            params Color[] values) : base(target,
            property,
            duration,
            new ColorLinearInterpolator(), easerFunction, useStartValue, values)
        {
        }
    }
}