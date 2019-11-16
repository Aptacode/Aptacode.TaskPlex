using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;
using System;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public class IntTransformation : PropertyTransformation<int>
    {
        /// <summary>
        /// Returns the easing function for this transformation
        /// </summary>
        readonly Easer _easer;

        /// <summary>
        /// Transform an int property on the target object to the value returned by the given Func at intervals
        /// specified by     the step duration up to the task duration
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <param name="valueUpdater"></param>
        /// <param name="taskDuration"></param>
        /// <param name="stepDuration"></param>
        public IntTransformation(object target,
                                 string property,
                                 Func<int> startValue,
                                 Func<int> endValue,
                                 Action<int> valueUpdater,
                                 TimeSpan taskDuration,
                                 TimeSpan stepDuration) : this(target,
                                                               property,
                                                               startValue,
                                                               endValue,
                                                               valueUpdater,
                                                               taskDuration,
                                                               stepDuration,
                                                               new LinearEaser())
        { }

        public IntTransformation(object target,
                                 string property,
                                 Func<int> startValue,
                                 Func<int> endValue,
                                 Action<int> valueUpdater,
                                 TimeSpan taskDuration,
                                 TimeSpan stepDuration,
                                 Easer easer) : base(target,
                                                     property,
                                                     startValue,
                                                     endValue,
                                                     valueUpdater,
                                                     taskDuration,
                                                     stepDuration) => _easer = easer;

        protected override async Task InternalTask()
        {
            var startValue = GetStartValue();
            var endValue = GetEndValue();

            var interpolator = new IntInterpolator(startValue, endValue, Duration, StepDuration, _easer);
            var values = interpolator.GetValues();
            StepTimer.Restart();

            for(var i = 0; i < values.Count; i++)
            {
                await WaitUntilResumed();
                SetValue(values[i]);
                await DelayAsync(i).ConfigureAwait(false);
            }

            SetValue(endValue);
        }
    }
}