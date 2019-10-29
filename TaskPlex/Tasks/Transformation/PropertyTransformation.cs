using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public abstract class PropertyTransformation : BaseTask
    {
        protected readonly Stopwatch StepTimer;

        protected PropertyTransformation(
            object target,
            string property,
            TimeSpan duration,
            TimeSpan stepDuration) : base(duration)
        {
            Target = target;
            Property = target.GetType().GetProperty(property);
            StepDuration = stepDuration;
            StepTimer = new Stopwatch();
        }

        /// <summary>
        ///     the object who's property is to be transformed
        /// </summary>
        public object Target { get; }

        /// <summary>
        ///     The property to be updated
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        ///     The time between each property update
        /// </summary>
        public TimeSpan StepDuration { get; set; }

        public override int GetHashCode()
        {
            return (Target.GetHashCode(), Property.GetHashCode()).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is PropertyTransformation other && StepTimer.Equals(other.StepTimer);
        }

        protected async Task DelayAsync(int currentStep)
        {
            var millisecondsAhead =
                (int) (StepDuration.TotalMilliseconds * currentStep - StepTimer.ElapsedMilliseconds);
            //the Task.Delay function will only accurately sleep for >8ms
            if (millisecondsAhead > 8)
            {
                await Task.Delay(millisecondsAhead, CancellationToken.Token).ConfigureAwait(false);
            }
        }
    }

    public abstract class PropertyTransformation<T> : PropertyTransformation
    {
        private readonly Func<T> _endValue;
        private readonly Action<T> _valueUpdater;

        protected PropertyTransformation(
            object target,
            string property,
            Func<T> endValue,
            Action<T> valueUpdater,
            TimeSpan duration,
            TimeSpan stepDuration) : base(target, property, duration, stepDuration)
        {
            _endValue = endValue;
            _valueUpdater = valueUpdater;
        }

        /// <summary>
        ///     When invoked returns the destination value of the transformation
        /// </summary>
        protected T GetStartValue()
        {
            return (T) Property.GetValue(Target);
        }

        protected T GetEndValue()
        {
            return _endValue.Invoke();
        }

        protected void SetValue(T value)
        {
            _valueUpdater.Invoke(value);
        }
    }
}