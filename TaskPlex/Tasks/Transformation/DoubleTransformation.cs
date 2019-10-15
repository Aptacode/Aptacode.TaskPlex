using System;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public class DoubleTransformationEventArgs : BaseTaskEventArgs
    {
    }

    public class DoubleTransformation : PropertyTransformation<double>
    {
        public Easer Easer { get; set; }
        public DoubleTransformation(object target, string property, Func<double> destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            Easer = new LinearEaser();
        }

        public DoubleTransformation(object target, string property, double destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            Easer = new LinearEaser();
        }
        public override async Task StartAsync()
        { 
            RaiseOnStarted(new DoubleTransformationEventArgs());

            DoubleInterpolator interpolator = new DoubleInterpolator(GetStartValue(), GetEndValue(), Duration, StepDuration);

            interpolator.SetEaser(Easer);

            interpolator.OnValueChanged += (s, e) =>
            {
                SetValue(e.Value);
            };

            await interpolator.StartAsync().ConfigureAwait(false);
            
            RaiseOnFinished(new DoubleTransformationEventArgs());

        }
    }
}
