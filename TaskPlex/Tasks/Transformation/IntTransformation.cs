using System;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public sealed class IntTransformation<TClass> : PropertyTransformation<TClass, int> where TClass : class
    {
        private IntTransformation(TClass target,
            string property,
            Func<int> endValue,
            TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal) : base(target,
            property,
            endValue,
            duration,
            refreshRate)
        {
        }

        /// <summary>
        ///     Returns the easing function for this transformation
        /// </summary>
        public Easer Easer { get; set; } = new LinearEaser();

        /// <summary>
        ///     Transform an int property on the target object to the value returned by the given Func at intervals
        ///     specified by     the step duration up to the task duration
        /// </summary>
        public static IntTransformation<T> Create<T>(T target, string property, int endValue, TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            try
            {
                return new IntTransformation<T>(target, property, () => endValue, duration, refreshRate);
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
            foreach (var value in new IntInterpolator().Interpolate(startValue, endValue, stepCount, Easer))
            {
                await WaitUntilResumed().ConfigureAwait(false);
                SetValue(value);
                await DelayAsync(stepIndex++).ConfigureAwait(false);
            }

            SetValue(endValue);
        }
    }
}