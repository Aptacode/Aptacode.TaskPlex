using System;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public class IntTransformation : PropertyTransformation<int>
    {
        /// <summary>
        ///     Transform an int property on the target object to the value returned by the given Func<> at intervals specified by
        ///     the step duration up to the task duration
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        /// <param name="destinationValue"></param>
        /// <param name="taskDuration"></param>
        /// <param name="stepDuration"></param>
        public IntTransformation(object target, string property, Func<int> destinationValue, TimeSpan taskDuration,
            TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            Easer = new LinearEaser();
        }

        /// <summary>
        ///     Transform an int property on the target object to the value returned by the given Func<> at intervals specified by
        ///     the step duration up to the task duration
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        /// <param name="destinationValue"></param>
        /// <param name="taskDuration"></param>
        /// <param name="stepDuration"></param>
        public IntTransformation(object target, string property, int destinationValue, TimeSpan taskDuration,
            TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            Easer = new LinearEaser();
        }

        /// <summary>
        ///     Returns the easing function for this transformation
        /// </summary>
        public Easer Easer { get; set; }

        protected override async Task InternalTask()
        {
            var interpolator = new IntInterpolator(GetStartValue(), GetEndValue(), Duration, StepDuration);

            interpolator.Easer = Easer;

            interpolator.OnValueChanged += (s, e) => { SetValue(e.Value); };
            await Task.WhenAll(interpolator.StartAsync(_cancellationToken)).ConfigureAwait(false);
        }
    }
}