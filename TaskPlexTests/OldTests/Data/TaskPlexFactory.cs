using System;
using System.Collections.Generic;
using System.Drawing;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;

namespace Aptacode.TaskPlex.Tests.OldTests.Data
{
    public static class TaskPlexFactory
    {
        public static IntTransformation GetIntTransformation(object testObject, string testProperty, int startValue,
            int endValue, int totalTime, int stepTime)
        {
            var property = testObject.GetType().GetProperty(testProperty);
            property?.SetValue(testObject, startValue);

            return GetIntTransformation(testObject, testProperty, endValue, totalTime, stepTime);
        }


        public static IntTransformation GetIntTransformation(object testObject, string testProperty, int endValue,
            int totalTime, int stepTime)
        {
            var property = testObject.GetType().GetProperty(testProperty);

            var transformation = new IntTransformation(
                testObject,
                testProperty,
                () => (int)property?.GetValue(testObject),
                () => endValue,
                value => property?.SetValue(testObject, value),
                TimeSpan.FromMilliseconds(totalTime),
                RefreshRate.Highest);

            return transformation;
        }

        public static DoubleTransformation GetDoubleTransformation(object testObject, string testProperty,
            double startValue, double endValue, int totalTime, int stepTime)
        {
            var property = testObject.GetType().GetProperty(testProperty);
            property?.SetValue(testObject, startValue);

            return GetDoubleTransformation(testObject, testProperty, endValue, totalTime, stepTime);
        }

        public static DoubleTransformation GetDoubleTransformation(object testObject, string testProperty,
            double endValue, int totalTime, int stepTime)
        {
            var property = testObject.GetType().GetProperty(testProperty);
            return new DoubleTransformation(
                testObject,
                testProperty,
                () => (double)property?.GetValue(testObject),
                () => endValue,
                value => property?.SetValue(testObject, value),
                TimeSpan.FromMilliseconds(totalTime),
                RefreshRate.Highest);
        }

        public static StringTransformation GetStringTransformation(object testObject, string testProperty,
            string startValue, string endValue, int totalTime, int stepTime)
        {
            var property = testObject.GetType().GetProperty(testProperty);
            property?.SetValue(testObject, startValue);

            var transformation = new StringTransformation(
                testObject,
                testProperty,
                () => (string)property?.GetValue(testObject),
                () => endValue,
                value => property?.SetValue(testObject, value),
                TimeSpan.FromMilliseconds(totalTime)
                );

            return transformation;
        }

        public static ColorTransformation GetColorTransformation(object testObject, string testProperty,
            Color startValue, Color endValue, int totalTime, int stepTime)
        {
            var property = testObject.GetType().GetProperty(testProperty);
            property?.SetValue(testObject, startValue);

            var transformation = new ColorTransformation(
                testObject,
                testProperty,
                () => (Color)property?.GetValue(testObject),
                () => endValue,
                value => property?.SetValue(testObject, value),
                TimeSpan.FromMilliseconds(totalTime),
                RefreshRate.Highest);

            return transformation;
        }

        public static SequentialGroupTask GetSequentialGroup() => GetSequentialGroup(
                new List<BaseTask>
                {
                    GetWaitTask(),
                    GetWaitTask()
                });

        public static SequentialGroupTask GetSequentialGroup(List<BaseTask> tasks) => new SequentialGroupTask(tasks);

        public static ParallelGroupTask GetParallelGroup() => GetParallelGroup(
                new List<BaseTask>
                {
                    GetWaitTask(),
                    GetWaitTask()
                });

        public static ParallelGroupTask GetParallelGroup(List<BaseTask> tasks) => new ParallelGroupTask(tasks);

        public static BaseTask GetWaitTask(int duration) => new WaitTask(TimeSpan.FromMilliseconds(duration));

        public static BaseTask GetWaitTask() => new WaitTask(TimeSpan.FromMilliseconds(0));

        public static object GetIntInterpolator(int startValue, int endValue, int total, int interval) => new IntInterpolator(startValue, endValue, TimeSpan.FromMilliseconds(total),
                RefreshRate.Highest);

        public static object GetDoubleInterpolator(double startValue, double endValue, int total, int interval) => new DoubleInterpolator(startValue, endValue, TimeSpan.FromMilliseconds(total),
                RefreshRate.Highest);
    }
}