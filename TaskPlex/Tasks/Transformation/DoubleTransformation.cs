using System;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public sealed class DoubleTransformation<TClass> : PropertyTransformation<TClass, double> where TClass : class
    {
        /// <summary>
        ///     Transform a double property on the target object to the value returned by the given Func<>at intervals
        ///     specified by the step duration up to the task duration
        /// </summary>
        /// <summary>
        private DoubleTransformation(TClass target,
            string property,
            Func<double> endValue,
            TimeSpan taskDuration,
            RefreshRate refreshRate = RefreshRate.Normal) : base(target,
            property,
            endValue,
            taskDuration,
            refreshRate)
        {
        }

        public Easer Easer { get; set; } = new LinearEaser();

        public static DoubleTransformation<T> Create<T>(T target, string property, double endValue, TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            try
            {
                return new DoubleTransformation<T>(target, property, () => endValue, duration, refreshRate);
            }
            catch
            {
                return null;
            }
        }

        protected override async Task InternalTask()
        {
            var startValue = GetValue();
            var endValue = GetEndValue();
            var stepCount = GetStepCount();

            Stopwatch.Restart();

            var stepIndex = 0;
            foreach (var value in new DoubleInterpolator().Interpolate(startValue, endValue, stepCount, Easer))
            {
                await WaitUntilResumed().ConfigureAwait(false);
                SetValue(value);
                await DelayAsync(stepIndex++).ConfigureAwait(false);
            }

            SetValue(endValue);
        }
    }
}