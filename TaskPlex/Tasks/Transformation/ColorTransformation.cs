using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using System;
using System.Drawing;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public class ColorTransformation : PropertyTransformation<Color>
    {
        /// <summary>
        /// Transform a Color property on the target object to the value returned by the given Func at intervals
        /// specified by     the step duration up to the task duration
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <param name="valueUpdater"></param>
        /// <param name="taskDuration"></param>
        /// <param name="stepDuration"></param>
        public ColorTransformation(object target,
                                   string property,
                                   Func<Color> startValue,
                                   Func<Color> endValue,
                                   Action<Color> valueUpdater,
                                   TimeSpan taskDuration,
                                   RefreshRate refreshRate) : base(target,
                                                                 property,
                                                                 startValue,
                                                                 endValue,
                                                                 valueUpdater,
                                                                 taskDuration,
                                                                 refreshRate) => Easer = new LinearEaser();

        /// <summary>
        /// Returns the easing function for this transformation
        /// </summary>
        public Easer Easer { get; set; }

        protected override async Task InternalTask()
        {
            var startValue = GetStartValue();
            var endValue = GetEndValue();

            var aComponentInterpolator = new IntInterpolator(startValue.A, endValue.A, Duration, RefreshRate, Easer);
            var rComponentInterpolator = new IntInterpolator(startValue.R, endValue.R, Duration, RefreshRate, Easer);
            var gComponentInterpolator = new IntInterpolator(startValue.G, endValue.G, Duration, RefreshRate, Easer);
            var bComponentInterpolator = new IntInterpolator(startValue.B, endValue.B, Duration, RefreshRate, Easer);

            var aValues = aComponentInterpolator.GetValues();
            var rValues = rComponentInterpolator.GetValues();
            var gValues = gComponentInterpolator.GetValues();
            var bValues = bComponentInterpolator.GetValues();

            StepTimer.Restart();

            for(var i = 0; i < aValues.Count; i++)
            {
                await WaitUntilResumed();

                SetValue(Color.FromArgb(aValues[i], rValues[i], gValues[i], bValues[i]));
                await DelayAsync(i).ConfigureAwait(false);
            }

            SetValue(endValue);
        }
    }
}