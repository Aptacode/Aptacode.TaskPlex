using System;
using System.Collections.Generic;
using System.Drawing;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;

namespace Aptacode.TaskPlex.Tests.Data
{
    public static class TaskPlexFactory
    {
        public static IntTransformation GetIntTransformation(object testObject, string testProperty, int startValue,
            int endValue, int totalTime, int stepTime)
        {
            var property = testObject.GetType().GetProperty(testProperty);
            property.SetValue(testObject, startValue);

            return GetIntTransformation(testObject, testProperty, endValue, totalTime, stepTime);
        }


        public static IntTransformation GetIntTransformation(object testObject, string testProperty, int endValue,
            int totalTime, int stepTime)
        {
            var transformation = new IntTransformation(
                testObject,
                testProperty,
                endValue,
                TimeSpan.FromMilliseconds(totalTime),
                TimeSpan.FromMilliseconds(stepTime));

            return transformation;
        }

        public static DoubleTransformation GetDoubleTransformation(object testObject, string testProperty,
            double startValue, double endValue, int totalTime, int stepTime)
        {
            var property = testObject.GetType().GetProperty(testProperty);
            property.SetValue(testObject, startValue);

            return GetDoubleTransformation(testObject, testProperty, endValue, totalTime, stepTime);
        }

        public static DoubleTransformation GetDoubleTransformation(object testObject, string testProperty,
            double endValue, int totalTime, int stepTime)
        {
            var transformation = new DoubleTransformation(
                testObject,
                testProperty,
                endValue,
                TimeSpan.FromMilliseconds(totalTime),
                TimeSpan.FromMilliseconds(stepTime));

            return transformation;
        }

        public static StringTransformation GetStringTransformation(object testObject, string testProperty,
            string startValue, string endValue, int totalTime, int stepTime)
        {
            var property = testObject.GetType().GetProperty(testProperty);
            property.SetValue(testObject, startValue);

            var transformation = new StringTransformation(
                testObject,
                testProperty,
                endValue,
                TimeSpan.FromMilliseconds(totalTime),
                TimeSpan.FromMilliseconds(stepTime));

            return transformation;
        }

        public static ColorTransformation GetColorTransformation(object testObject, string testProperty,
            Color startValue, Color endValue, int totalTime, int stepTime)
        {
            var property = testObject.GetType().GetProperty(testProperty);
            property.SetValue(testObject, startValue);

            var transformation = new ColorTransformation(
                testObject,
                testProperty,
                endValue,
                TimeSpan.FromMilliseconds(totalTime),
                TimeSpan.FromMilliseconds(stepTime));

            return transformation;
        }

        public static SequentialGroupTask GetSequentialGroup()
        {
            return GetSequentialGroup(
                new List<IBaseTask>
                {
                    GetWaitTask(),
                    GetWaitTask()
                });
        }

        public static SequentialGroupTask GetSequentialGroup(List<IBaseTask> tasks)
        {
            return new SequentialGroupTask(tasks);
        }

        public static ParallelGroupTask GetParallelGroup()
        {
            return GetParallelGroup(
                new List<IBaseTask>
                {
                    GetWaitTask(),
                    GetWaitTask()
                });
        }

        public static ParallelGroupTask GetParallelGroup(List<IBaseTask> tasks)
        {
            return new ParallelGroupTask(tasks);
        }

        public static BaseTask GetWaitTask(int duration)
        {
            return new WaitTask(TimeSpan.FromMilliseconds(duration));
        }

        public static BaseTask GetWaitTask()
        {
            return new WaitTask(TimeSpan.FromMilliseconds(0));
        }

        public static object GetIntInterpolator(int startValue, int endValue, int total, int interval)
        {
            return new IntInterpolator(startValue, endValue, TimeSpan.FromMilliseconds(total),
                TimeSpan.FromMilliseconds(interval));
        }

        public static object GetDoubleInterpolator(double startValue, double endValue, int total, int interval)
        {
            return new DoubleInterpolator(startValue, endValue, TimeSpan.FromMilliseconds(total),
                TimeSpan.FromMilliseconds(interval));
        }
    }
}