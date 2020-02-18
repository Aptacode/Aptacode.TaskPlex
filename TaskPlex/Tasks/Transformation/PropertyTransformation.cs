using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public enum RefreshRate
    {
        Low = 32, Normal = 16, High = 8, Highest = 1
    }

    public abstract class PropertyTransformation : BaseTask
    {
        protected readonly Stopwatch StepTimer;

        protected PropertyTransformation(object target, string property, TimeSpan duration, RefreshRate refreshRate) : base(duration)
        {
            Target = target;
            Property = property;
            RefreshRate = refreshRate;
            StepTimer = new Stopwatch();
        }

        /// <summary>
        /// the object who's property is to be transformed
        /// </summary>
        public object Target { get; }

        /// <summary>
        /// The property to be updated
        /// </summary>
        public string Property { get; }

        /// <summary>
        /// The time between each property update
        /// </summary>
        protected RefreshRate RefreshRate { get; }

        public override int GetHashCode() => (Target, Property).GetHashCode();

        public override bool Equals(object obj) => (obj is PropertyTransformation other) &&
            StepTimer.Equals(other.StepTimer);

        protected async Task DelayAsync(int currentStep)
        {
            int millisecondsAhead =
                ((int)RefreshRate * currentStep) - (int)StepTimer.ElapsedMilliseconds;
            //the Task.Delay function will only accurately sleep for >8ms
            if(millisecondsAhead > 8)
            {
                await Task.Delay(millisecondsAhead, CancellationToken.Token).ConfigureAwait(false);
            }
        }

        protected int GetStepCount() => (int)Math.Floor(((double)Duration.TotalMilliseconds) / (int)RefreshRate);
    }

    public abstract class PropertyTransformation<T> : PropertyTransformation
    {
        readonly Func<T> _endValue;
        readonly Func<T> _startValue;
        readonly Action<T> _valueUpdater;

        protected PropertyTransformation(object target,
                                         string property,
                                         Func<T> startValue,
                                         Func<T> endValue,
                                         Action<T> valueUpdater,
                                         TimeSpan duration,
                                         RefreshRate refreshRate) : base(target, property, duration, refreshRate)
        {
            _startValue = startValue;
            _endValue = endValue;
            _valueUpdater = valueUpdater;
        }

        /// <summary>
        /// When invoked returns the destination value of the transformation
        /// </summary>
        protected T GetStartValue() => _startValue.Invoke();

        protected T GetEndValue() => _endValue.Invoke();

        protected void SetValue(T value) => _valueUpdater.Invoke(value);
    }
}