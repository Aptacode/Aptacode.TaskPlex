using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public sealed class ColorTransformation<TClass> : PropertyTransformation<TClass, Color> where TClass : class
    {
        /// <summary>
        ///     Transform a Color property on the target object to the value returned by the given Func at intervals
        ///     specified by     the step duration up to the task duration
        /// </summary>
        private ColorTransformation(TClass target,
            string property,
            Func<Color> endValue,
            TimeSpan taskDuration,
            RefreshRate refreshRate = RefreshRate.Normal) : base(target,
            property,
            endValue,
            taskDuration,
            refreshRate)
        {
        }

        /// <summary>
        ///     Returns the easing function for this transformation
        /// </summary>
        public Easer Easer { get; set; } = new LinearEaser();

        public static ColorTransformation<T> Create<T>(T target, string property, Color endValue, TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            try
            {
                return new ColorTransformation<T>(target, property, () => endValue, duration, refreshRate);
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

            var intInterpolator = new IntInterpolator();
            var aValues = intInterpolator.Interpolate(startValue.A, endValue.A, stepCount, Easer);
            var rValues = intInterpolator.Interpolate(startValue.R, endValue.R, stepCount, Easer);
            var gValues = intInterpolator.Interpolate(startValue.G, endValue.G, stepCount, Easer);
            var bValues = intInterpolator.Interpolate(startValue.B, endValue.B, stepCount, Easer);

            Stopwatch.Restart();

            for (var i = 0; i < aValues.Count(); i++)
            {
                await WaitUntilResumed().ConfigureAwait(false);

                SetValue(Color.FromArgb(aValues.ElementAt(i), rValues.ElementAt(i), gValues.ElementAt(i),
                    bValues.ElementAt(i)));
                await DelayAsync(i).ConfigureAwait(false);
            }

            SetValue(endValue);
        }
    }
}