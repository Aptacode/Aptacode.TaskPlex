using System;
using System.Windows;
using System.Windows.Media;
using Aptacode.TaskPlex.Interpolation.Easers;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation
{
    public static class WPFTransformationFactory
    {
        public static PointTransformation<T> Create<T>(T target, string property, TimeSpan duration,
            EaserFunction easerFunction = null, bool useStartValue = true,
            params Point[] values) where T : class
        {
            return new PointTransformation<T>(target, property,
                duration, easerFunction, useStartValue, values);
        }

        public static ColorTransformation<T> Create<T>(T target, string property, TimeSpan duration,
            EaserFunction easerFunction = null, bool useStartValue = true,
            params Color[] values) where T : class
        {
            return new ColorTransformation<T>(target, property,
                duration, easerFunction, useStartValue, values);
        }

        public static ThicknessTransformation<T> Create<T>(T target, string property,
            TimeSpan duration, EaserFunction easerFunction = null, bool useStartValue = true,
            params Thickness[] values) where T : class
        {
            return new ThicknessTransformation<T>(target, property,
                duration, easerFunction, useStartValue, values);
        }
    }
}