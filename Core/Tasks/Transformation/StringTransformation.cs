using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Aptacode.Core.Tasks.Transformations
{
    public class StringTransformEventArgs : BaseTaskEventArgs
    {
        public StringTransformEventArgs()
        {

        }
    }

    public class StringTransformation : PropertyTransformation<string>
    {
        private Stopwatch stepTimer;
        public StringTransformation(object target, string property, Func<string> destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            stepTimer = new Stopwatch();
        }

        public StringTransformation(object target, string property, string destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            stepTimer = new Stopwatch();
        }

        public override async Task StartAsync()
        {
            RaiseOnStarted(new StringTransformEventArgs());

            await Task.Delay(TaskDuration);

            SetValue(GetEndValue());

            RaiseOnFinished(new StringTransformEventArgs());

        }
    }
}
