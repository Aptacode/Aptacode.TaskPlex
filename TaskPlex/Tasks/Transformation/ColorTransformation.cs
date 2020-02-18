using System;
using System.Drawing;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public class ColorTransformation<TClass> : PropertyTransformation<TClass, Color> where TClass : class
    {
        /// <summary>
        ///     Transform a Color property on the target object to the value returned by the given Func at intervals
        ///     specified by     the step duration up to the task duration
        /// </summary>
        public ColorTransformation(TClass target,
            string property,
            Color endValue,
            TimeSpan taskDuration,
            RefreshRate refreshRate = RefreshRate.Normal) : base(target,
            property,
            endValue,
            taskDuration,
            refreshRate)
        {
        }

        public ColorTransformation(TClass target,
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

            StepTimer.Restart();

            for (var i = 0; i < aValues.Count; i++)
            {
                await WaitUntilResumed();

                SetValue(Color.FromArgb(aValues[i], rValues[i], gValues[i], bValues[i]));
                await DelayAsync(i).ConfigureAwait(false);
            }

            SetValue(endValue);
        }
    }
}