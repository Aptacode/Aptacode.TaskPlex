using System;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation.EventArgs;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public class DoubleTransformation : PropertyTransformation<double>
    {
        /// <summary>
        ///     Transform a double property on the target object to the value returned by the given Func<> at intervals specified
        ///     by the step duration up to the task duration
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        /// <param name="destinationValue"></param>
        /// <param name="taskDuration"></param>
        /// <param name="stepDuration"></param>
        public DoubleTransformation(object target, string property, Func<double> destinationValue,
            TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration,
            stepDuration)
        {
            Easer = new LinearEaser();
        }

        /// <summary>
        ///     Transform a double property on the target object to the value returned by the given Func<> at intervals specified
        ///     by the step duration up to the task duration
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        /// <param name="destinationValue"></param>
        /// <param name="taskDuration"></param>
        /// <param name="stepDuration"></param>
        public DoubleTransformation(object target, string property, double destinationValue, TimeSpan taskDuration,
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
            try
            {
                RaiseOnStarted(new DoubleTransformationEventArgs());

                var interpolator = new DoubleInterpolator(GetStartValue(), GetEndValue(), Duration, StepDuration);

                interpolator.Easer = Easer;

                interpolator.OnValueChanged += (s, e) => { SetValue(e.Value); };

                await interpolator.StartAsync(_cancellationToken).ConfigureAwait(false);

                RaiseOnFinished(new DoubleTransformationEventArgs());
            }
            catch (TaskCanceledException)
            {
                RaiseOnCancelled();
            }
        }
    }
}