using Aptacode.Core.Tasks.Transformations;
using Aptacode.Core.Tasks.Transformations.Interpolation;
using System;
using System.Reflection;

namespace Aptacode.TaskPlex.Core_Tests.Utilites
{
    public static class PropertyTransformation_Helpers
    {
        public static PropertyTransformation GetIntInterpolation(object testObject, string testProperty, int startValue, int endValue, int totalTime, int stepTime)
        {
            PropertyInfo property = testObject.GetType().GetProperty(testProperty);
            property.SetValue(testObject, startValue);

            PropertyTransformation transformation = new IntInterpolation(
                testObject,
                property,
                endValue,
                TimeSpan.FromMilliseconds(totalTime),
                TimeSpan.FromMilliseconds(stepTime));

            return transformation;
        }


        public static PropertyTransformation GetIntInterpolation(object testObject, string testProperty, int endValue, int totalTime, int stepTime)
        {
            PropertyInfo property = testObject.GetType().GetProperty(testProperty);

            PropertyTransformation transformation = new IntInterpolation(
                testObject,
                property,
                endValue,
                TimeSpan.FromMilliseconds(totalTime),
                TimeSpan.FromMilliseconds(stepTime));

            return transformation;
        }

        public static PropertyTransformation GetDoubleInterpolation(object testObject, string testProperty, double startValue, double endValue, int totalTime, int stepTime)
        {
            PropertyInfo property = testObject.GetType().GetProperty(testProperty);
            property.SetValue(testObject, startValue);

            PropertyTransformation transformation = new DoubleInterpolation(
                testObject,
                property,
                endValue,
                TimeSpan.FromMilliseconds(totalTime),
                TimeSpan.FromMilliseconds(stepTime));

            return transformation;
        }

        public static PropertyTransformation GetDoubleInterpolation(object testObject, string testProperty, double endValue, int totalTime, int stepTime)
        {
            PropertyInfo property = testObject.GetType().GetProperty(testProperty);

            PropertyTransformation transformation = new DoubleInterpolation(
                testObject,
                property,
                endValue,
                TimeSpan.FromMilliseconds(totalTime),
                TimeSpan.FromMilliseconds(stepTime));

            return transformation;
        }
    }
}
