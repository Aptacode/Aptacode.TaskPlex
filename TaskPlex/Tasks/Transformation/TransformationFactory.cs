using System;
using System.Drawing;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public static class TransformationFactory
    {
        public static IntTransformation<T> Create<T>(T target, string property, int endValue, TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            return IntTransformation<T>.Create(target, property, endValue, duration, refreshRate);
        }

        public static DoubleTransformation<T> Create<T>(T target, string property, double endValue, TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            return DoubleTransformation<T>.Create(target, property, endValue, duration, refreshRate);
        }

        public static StringTransformation<T> Create<T>(T target, string property, string endValue, TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            return StringTransformation<T>.Create(target, property, endValue, duration, refreshRate);
        }

        public static ColorTransformation<T> Create<T>(T target, string property, Color endValue, TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            return ColorTransformation<T>.Create(target, property, endValue, duration, refreshRate);
        }

        public static BoolTransformation<T> Create<T>(T target, string property, bool endValue, TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            return BoolTransformation<T>.Create(target, property, endValue, duration, refreshRate);
        }
    }
}