using System;
using System.Windows.Media;
using Aptacode.TaskPlex.Engine.Tasks.Transformations.Interpolation;
using Aptacode.TaskPlex.Interpolation.Easers;
using Aptacode.TaskPlex.WPF.Tasks.Transformation.Interpolator;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation
{
    public sealed class ColorTransformation<TClass> : InterpolatedTransformation<TClass, Color> where TClass : class
    {
        public ColorTransformation(TClass target,
            string property,
            TimeSpan duration,
            EaserFunction easerFunction = null,
            bool useStartValue = true,
            params Color[] values) : base(target,
            property,
            duration,
            new ColorInterpolator(),
            easerFunction, useStartValue, values)
        {
        }
    }
}