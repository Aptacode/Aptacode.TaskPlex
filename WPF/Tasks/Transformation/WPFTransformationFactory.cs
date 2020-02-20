using System;
using System.Windows;
using System.Windows.Media;
using Aptacode.TaskPlex.Tasks.Transformation;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation
{
    public static class WPFTransformationFactory
    {
        public static PointTransformation<T> Create<T>(T target, string property, Point endValue, TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            return PointTransformation<T>.Create(target, property, endValue, duration, refreshRate);
        }

        public static ColorTransformation<T> Create<T>(T target, string property, Color endValue, TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            return ColorTransformation<T>.Create(target, property, endValue, duration, refreshRate);
        }
    }
}