using System;
using System.Drawing;
using System.Linq;
using Aptacode.TaskPlex.Enums;
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
            return new WaitTask((int) (duration.TotalMilliseconds / (int) RefreshRate.High));
        }

        public static IntTransformation<T> Create<T>(T target, string property, int endValue, TimeSpan duration,
            EaserFunction easerFunction = null) where T : class
        {
            return IntTransformation<T>.Create(target, property, endValue,
                (int) (duration.TotalMilliseconds / (int) RefreshRate.High), easerFunction);
        }

        public static DoubleTransformation<T> Create<T>(T target, string property, double endValue, TimeSpan duration,
            EaserFunction easerFunction = null) where T : class
        {
            return DoubleTransformation<T>.Create(target, property, endValue,
                (int) (duration.TotalMilliseconds / (int) RefreshRate.High), easerFunction);
        }

        public static StringTransformation<T> Create<T>(T target, string property, string endValue, TimeSpan duration)
            where T : class
        {
            return StringTransformation<T>.Create(target, property, endValue,
                (int) (duration.TotalMilliseconds / (int) RefreshRate.High));
        }

        public static ColorTransformation<T> Create<T>(T target, string property, Color endValue, TimeSpan duration,
            EaserFunction easerFunction = null) where T : class
        {
            return ColorTransformation<T>.Create(target, property, endValue,
                (int) (duration.TotalMilliseconds / (int) RefreshRate.High), easerFunction);
        }

        public static PointTransformation<T> Create<T>(T target, string property, Point endValue, TimeSpan duration,
            EaserFunction easerFunction = null) where T : class
        {
            return PointTransformation<T>.Create(target, property, endValue,
                (int) (duration.TotalMilliseconds / (int) RefreshRate.High), easerFunction);
        }

        public static BoolTransformation<T> Create<T>(T target, string property, bool endValue, TimeSpan duration)
            where T : class
        {
            return BoolTransformation<T>.Create(target, property, endValue,
                (int) (duration.TotalMilliseconds / (int) RefreshRate.High));
        }
    }
}