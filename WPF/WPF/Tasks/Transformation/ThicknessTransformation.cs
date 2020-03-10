using System;
using System.Windows;
using Aptacode.TaskPlex.Engine.Tasks.Transformations.Interpolation;
using Aptacode.TaskPlex.WPF.Tasks.Transformation.Interpolator;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation
{
    public sealed class ThicknessTransformation<TClass> : InterpolatedTransformation<TClass, Thickness>
        where TClass : class
    {
        public ThicknessTransformation(TClass target,
            string property,
            TimeSpan duration,
            EaserFunction easerFunction = null,
            bool useStartValue = true,
            params Thickness[] values) : base(target,
            property,
            duration,
            new ThicknessLinearInterpolator(),
            easerFunction, useStartValue, values)
        {
        }
    }
}