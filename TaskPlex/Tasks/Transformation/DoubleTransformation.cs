using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;
using System;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public class DoubleTransformation : PropertyTransformation<double>
    {
        readonly Easer _easer;

        /// <summary>
        /// Transform a double property on the target object to the value returned by the given Func<>at intervals
        /// specified by the step duration up to the task duration</summary> <param name="target"></param> <param
        /// name="property"></param> <param name="startValue"></param> <param name="endValue"></param> <param
        /// name="valueUpdater"></param> <param name="taskDuration"></param> <param name="stepDuration"></param>
      
        
        public DoubleTransformation(object target,
                                    string property,
                                    Func<double> startValue,
                                    Func<double> endValue,
                                    Action<double> valueUpdater,
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

        public DoubleTransformation(object target,
                                    string property,
                                    Func<double> startValue,
                                    Func<double> endValue,
                                    Action<double> valueUpdater,
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

            var interpolator = new DoubleInterpolator(startValue, endValue, Duration, StepDuration, _easer);
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