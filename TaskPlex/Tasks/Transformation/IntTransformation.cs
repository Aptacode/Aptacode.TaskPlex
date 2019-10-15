using System;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public class IntTransformationEventArgs : BaseTaskEventArgs
    {
    }

    public class IntTransformation : PropertyTransformation<int>
    {
        public Easer Easer { get; set; }

        public IntTransformation(object target, string property, Func<int> destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            Easer = new LinearEaser();
        }

        public IntTransformation(object target, string property, int destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
        }

        public override async Task StartAsync()
        {
            RaiseOnStarted(new IntTransformationEventArgs());
            IntInterpolator interpolator = new IntInterpolator(GetStartValue(), GetEndValue(), Duration, StepDuration);

            interpolator.SetEaser(Easer);

            interpolator.OnValueChanged += (s, e) =>
            {
                SetValue(e.Value);
            };

            await interpolator.StartAsync().ConfigureAwait(false); 
            RaiseOnFinished(new IntTransformationEventArgs());

        }
    }
}
