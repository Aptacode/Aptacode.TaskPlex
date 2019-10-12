using Aptacode.Core.Tasks.Transformations.Interpolation;
using System;
using System.Threading.Tasks;

namespace Aptacode.Core.Tasks.Transformations
{
    public class DoubleTransformationventArgs : BaseTaskEventArgs
    {
    }

    public class DoubleTransformation : PropertyTransformation<double>
    {
        public DoubleTransformation(object target, string property, Func<double> destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
        }

        public DoubleTransformation(object target, string property, double destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
        }

        public override async Task StartAsync()
        {
            RaiseOnStarted(new DoubleTransformationventArgs());

            DoubleInterpolator interpolator = new DoubleInterpolator(GetStartValue(), GetEndValue(), Duration, StepDuration);

            interpolator.OnValueChanged += (s, e) =>
            {
                SetValue(e.Value);
            };

            await interpolator.StartAsync().ConfigureAwait(false);

            RaiseOnFinished(new DoubleTransformationventArgs());

        }
    }
}
