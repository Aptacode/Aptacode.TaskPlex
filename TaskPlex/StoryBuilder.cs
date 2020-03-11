using System;
using System.Drawing;
using System.Linq;
using Aptacode.Interpolatr;
using Aptacode.TaskPlex.Stories;
using Aptacode.TaskPlex.Stories.Transformations;
using Aptacode.TaskPlex.Stories.Transformations.Interpolation;

namespace Aptacode.TaskPlex
{
    public static class StoryBuilder
    {
        public static SequentialGroupStory Sequential(params BaseStory[] tasks)
        {
            return new SequentialGroupStory(tasks.ToList());
        }

        public static ParallelGroupStory Parallel(params BaseStory[] tasks)
        {
            return new ParallelGroupStory(tasks.ToList());
        }

        public static RepeatStory Repeat(BaseStory story, int count)
        {
            return new RepeatStory(story, count);
        }

        public static WaitStory Wait(TimeSpan duration)
        {
            return new WaitStory(duration);
        }

        public static IntTransformation<T> Create<T>(T target, string property, TimeSpan duration,
            EaserFunction easerFunction = null, bool useStartValue = true,
            params int[] values) where T : class
        {
            return new IntTransformation<T>(target, property,
                duration, easerFunction, useStartValue, values);
        }

        public static DoubleTransformation<T> Create<T>(T target, string property, TimeSpan duration,
            EaserFunction easerFunction = null, bool useStartValue = true,
            params double[] values) where T : class
        {
            return new DoubleTransformation<T>(target, property,
                duration, easerFunction, useStartValue, values);
        }

        public static StringTransformation<T> Create<T>(T target, string property, TimeSpan duration, string endValue)
            where T : class
        {
            return new StringTransformation<T>(target, property, duration, endValue);
        }

        public static ColorTransformation<T> Create<T>(T target, string property, TimeSpan duration,
            EaserFunction easerFunction = null, bool useStartValue = true,
            params Color[] values) where T : class
        {
            return new ColorTransformation<T>(target, property, duration, easerFunction, useStartValue, values);
        }

        public static PointTransformation<T> Create<T>(T target, string property, TimeSpan duration,
            EaserFunction easerFunction = null, bool useStartValue = true,
            params Point[] values) where T : class
        {
            return new PointTransformation<T>(target, property,
                duration, easerFunction, useStartValue, values);
        }

        public static BoolTransformation<T> Create<T>(T target, string property, TimeSpan duration, bool value)
            where T : class
        {
            return new BoolTransformation<T>(target, property, duration, value);
        }
    }
}