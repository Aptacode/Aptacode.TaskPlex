using System;
using System.Drawing;
using Aptacode.Interpolatr;
using Aptacode.Interpolatr.Linear;

namespace Aptacode.TaskPlex.Stories.Transformations.Interpolation
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
            new ColorInterpolator(), easerFunction, useStartValue, values)
        {
        }
    }
}