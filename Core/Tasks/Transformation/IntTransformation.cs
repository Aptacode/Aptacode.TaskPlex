using Aptacode.Core.Tasks.Transformations.Interpolation;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aptacode.Core.Tasks.Transformations
{
    public class IntTransformationventArgs : BaseTaskEventArgs
    {
        public IntTransformationventArgs()
        {

        }
    }

    public class IntTransformation : PropertyTransformation<int>
    {
        public IntTransformation(object target, string property, Func<int> destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
        }

        public IntTransformation(object target, string property, int destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
        }

        public override async Task StartAsync()
        {
            RaiseOnStarted(new IntTransformationventArgs());

            IntInterpolator interpolator = new IntInterpolator(GetStartValue(), GetEndValue(), Duration, StepDuration);
            interpolator.OnValueChanged += (s, e) =>
            {
                SetValue(e.Value);
            };

            await interpolator.StartAsync();

            RaiseOnFinished(new IntTransformationventArgs());

        }
    }
}
