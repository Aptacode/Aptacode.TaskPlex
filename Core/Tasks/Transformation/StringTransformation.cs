using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aptacode.Core.Tasks.Transformations
{
    public class StringTransformEventArgs : BaseTaskEventArgs
    {
    }

    public class StringTransformation : PropertyTransformation<string>
    {
        public StringTransformation(object target, string property, Func<string> destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {

        }
        public StringTransformation(object target, string property, string destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            
        }
        public override async Task StartAsync()
        {
            RaiseOnStarted(new StringTransformEventArgs());

            await Task.Delay(Duration).ConfigureAwait(false);

            SetValue(GetEndValue());

            RaiseOnFinished(new StringTransformEventArgs());
        }
    }
}
