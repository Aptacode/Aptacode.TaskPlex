using System;
using System.Windows;
using System.Windows.Media;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation
{
    public static class WPFTransformationFactory
    {
        public static PointTransformation<T> Create<T>(T target, string property, TimeSpan duration,
            EaserFunction easerFunction = null, params Point[] values) where T : class
        {
            return new PointTransformation<T>(target, property,
                duration, easerFunction, values);
        }

        public static ColorTransformation<T> Create<T>(T target, string property, TimeSpan duration,
            EaserFunction easerFunction = null, params Color[] values) where T : class
        {
            return new ColorTransformation<T>(target, property,
                duration, easerFunction, values);
        }

        public static ThicknessTransformation<T> Create<T>(T target, string property,
            TimeSpan duration, EaserFunction easerFunction = null, params Thickness[] values) where T : class
        {
            return new ThicknessTransformation<T>(target, property,
                duration, easerFunction, values);
        }
    }
}