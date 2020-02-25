using System;
using System.Windows;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Interpolators.Easers;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolation;
using Aptacode.TaskPlex.WPF.Tasks.Transformation.Interpolator;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation
{
    public sealed class ThicknessTransformation<TClass> : InterpolatedTransformation<TClass, Thickness>
        where TClass : class
    {
        private ThicknessTransformation(TClass target,
            string property,
            Func<Thickness> endValue,
            TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal, EaserFunction easerFunction = null) : base(target,
            property,
            endValue,
            duration,
            new ThicknessInterpolator(),
            refreshRate, easerFunction)
        {
        }

        /// <summary>
        ///     Transform an int property on the target object to the value returned by the given Func at intervals
        ///     specified by     the step duration up to the task duration
        /// </summary>
        public static ThicknessTransformation<T> Create<T>(T target, string property, Thickness endValue,
            TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal, EaserFunction easerFunction = null) where T : class
        {
            try
            {
                return new ThicknessTransformation<T>(target, property, () => endValue, duration, refreshRate,
                    easerFunction);
            }
            catch
            {
                return null;
            }
        }
    }
}