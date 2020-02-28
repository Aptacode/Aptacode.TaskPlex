using System;
using System.Drawing;
using System.Linq;
using Aptacode.TaskPlex.Interpolators.Easers;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tasks.Transformations;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolation;

namespace Aptacode.TaskPlex
{
    public static class TaskPlexFactory
    {
        public static SequentialGroupTask Sequential(params BaseTask[] tasks)
        {
            return new SequentialGroupTask(tasks.ToList());
        }

        public static ParallelGroupTask Parallel(params BaseTask[] tasks)
        {
            return new ParallelGroupTask(tasks.ToList());
        }

        public static RepeatTask Repeat(BaseTask task, int count)
        {
            return new RepeatTask(task, count);
        }

        public static WaitTask Wait(TimeSpan duration)
        {
            return new WaitTask(duration);
        }

        public static IntTransformation<T> Create<T>(T target, string property, TimeSpan duration,
            EaserFunction easerFunction = null, params int[] values) where T : class
        {
            return new IntTransformation<T>(target, property,
                duration, easerFunction, values);
        }

        public static DoubleTransformation<T> Create<T>(T target, string property, TimeSpan duration,
            EaserFunction easerFunction = null, params double[] values) where T : class
        {
            return new DoubleTransformation<T>(target, property,
                duration, easerFunction, values);
        }

        public static StringTransformation<T> Create<T>(T target, string property, TimeSpan duration, string endValue)
            where T : class
        {
            return new StringTransformation<T>(target, property, duration, endValue);
        }

        public static ColorTransformation<T> Create<T>(T target, string property, TimeSpan duration,
            EaserFunction easerFunction = null, params Color[] values) where T : class
        {
            return new ColorTransformation<T>(target, property, duration, easerFunction, values);
        }

        public static PointTransformation<T> Create<T>(T target, string property, TimeSpan duration,
            EaserFunction easerFunction = null, params Point[] values) where T : class
        {
            return new PointTransformation<T>(target, property,
                duration, easerFunction, values);
        }

        public static BoolTransformation<T> Create<T>(T target, string property, TimeSpan duration, bool value)
            where T : class
        {
            return new BoolTransformation<T>(target, property, duration, value);
        }
    }
}