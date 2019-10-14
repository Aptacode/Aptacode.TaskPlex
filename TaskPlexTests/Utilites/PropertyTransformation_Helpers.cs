using System;
using System.Drawing;
using System.Reflection;
using Aptacode.TaskPlex.Tasks.Transformation;

namespace Aptacode.TaskPlex.Tests.Utilites
{
    public static class PropertyTransformationHelpers
    {
        public static IntTransformation GetIntTransformation(object testObject, string testProperty, int startValue, int endValue, int totalTime, int stepTime)
        {
            PropertyInfo property = testObject.GetType().GetProperty(testProperty);
            property.SetValue(testObject, startValue);

            return GetIntTransformation(testObject, testProperty, endValue, totalTime, stepTime);
        }


        public static IntTransformation GetIntTransformation(object testObject, string testProperty, int endValue, int totalTime, int stepTime)
        {
            IntTransformation transformation = new IntTransformation(
                testObject,
                testProperty,
                endValue,
                TimeSpan.FromMilliseconds(totalTime),
                TimeSpan.FromMilliseconds(stepTime));

            return transformation;
        }

        public static DoubleTransformation GetDoubleTransformation(object testObject, string testProperty, double startValue, double endValue, int totalTime, int stepTime)
        {
            PropertyInfo property = testObject.GetType().GetProperty(testProperty);
            property.SetValue(testObject, startValue);

            return GetDoubleTransformation(testObject, testProperty, endValue, totalTime, stepTime);
        }

        public static DoubleTransformation GetDoubleTransformation(object testObject, string testProperty, double endValue, int totalTime, int stepTime)
        {
            DoubleTransformation transformation = new DoubleTransformation(
                testObject,
                testProperty,
                endValue,
                TimeSpan.FromMilliseconds(totalTime),
                TimeSpan.FromMilliseconds(stepTime));

            return transformation;
        }

        public static StringTransformation GetStringTransformation(object testObject, string testProperty, string startValue, string endValue, int totalTime, int stepTime)
        {
            PropertyInfo property = testObject.GetType().GetProperty(testProperty);
            property.SetValue(testObject, startValue);

            StringTransformation transformation = new StringTransformation(
                testObject,
                testProperty,
                endValue,
                TimeSpan.FromMilliseconds(totalTime),
                TimeSpan.FromMilliseconds(stepTime));

            return transformation;
        }

        public static ColorTransformation GetColorTransformation(object testObject, string testProperty, Color startValue, Color endValue, int totalTime, int stepTime)
        {
            PropertyInfo property = testObject.GetType().GetProperty(testProperty);
            property.SetValue(testObject, startValue);

            ColorTransformation transformation = new ColorTransformation(
                testObject,
                testProperty,
                endValue,
                TimeSpan.FromMilliseconds(totalTime),
                TimeSpan.FromMilliseconds(stepTime));

            return transformation;
        }
    }
}
