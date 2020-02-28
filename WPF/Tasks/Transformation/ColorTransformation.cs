using System;
using System.Windows.Media;
using Aptacode.TaskPlex.Interpolators.Easers;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolation;
using Aptacode.TaskPlex.WPF.Tasks.Transformation.Interpolator;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation
{
    public sealed class ColorTransformation<TClass> : InterpolatedTransformation<TClass, Color> where TClass : class
    {
        public ColorTransformation(TClass target,
            string property,
            TimeSpan duration,
            EaserFunction easerFunction = null,
            params Color[] values) : base(target,
            property,
            duration,
            new ColorInterpolator(),
            easerFunction, values)
        {
        }
    }
}