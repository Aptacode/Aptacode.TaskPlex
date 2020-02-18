using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public enum RefreshRate
    {
        Low = 32,
        Normal = 16,
        High = 8,
        Highest = 1
    }

    public abstract class PropertyTransformation : BaseTask
    {
        protected readonly Stopwatch StepTimer;

        protected PropertyTransformation(TimeSpan duration, RefreshRate refreshRate) : base(duration)
        {
            RefreshRate = refreshRate;
            StepTimer = new Stopwatch();
        }

        /// <summary>
        ///     The time between each property update
        /// </summary>
        protected RefreshRate RefreshRate { get; }

        public new abstract int GetHashCode();

        public override bool Equals(object obj)
        {
            return obj is PropertyTransformation other &&
                   StepTimer.Equals(other.StepTimer);
        }

        protected async Task DelayAsync(int currentStep)
        {
            var millisecondsAhead =
                (int) RefreshRate * currentStep - (int) StepTimer.ElapsedMilliseconds;
            //the Task.Delay function will only accurately sleep for >8ms
            if (millisecondsAhead > 8)
            {
                await Task.Delay(millisecondsAhead, CancellationToken.Token).ConfigureAwait(false);
            }
        }

        protected int GetStepCount()
        {
            return (int) Math.Floor(Duration.TotalMilliseconds / (int) RefreshRate);
        }
    }

    public abstract class PropertyTransformation<TClass, TPropertyType> : PropertyTransformation where TClass : class
    {
        private readonly Func<TPropertyType> _endValue;
        private readonly Func<TClass, TPropertyType> _getter;
        private readonly Action<TClass, TPropertyType> _setter;

        protected PropertyTransformation(TClass target,
            string property,
            TPropertyType endValue,
            TimeSpan duration,
            RefreshRate refreshRate) : this(target, property, () => endValue, duration, refreshRate)
        {
        }

        protected PropertyTransformation(TClass target,
            string property,
            Func<TPropertyType> endValue,
            TimeSpan duration,
            RefreshRate refreshRate) : base(duration, refreshRate)
        {
            Target = target;
            Property = property;
            _setter = (Action<TClass, TPropertyType>) Delegate.CreateDelegate(typeof(Action<TClass, TPropertyType>),
                null, typeof(TClass).GetProperty(Property).GetSetMethod());
            _getter = (Func<TClass, TPropertyType>) Delegate.CreateDelegate(typeof(Func<TClass, TPropertyType>), null,
                typeof(TClass).GetProperty(Property).GetGetMethod());
            _endValue = endValue;
        }


        /// <summary>
        ///     the object who's property is to be transformed
        /// </summary>
        public TClass Target { get; }

        /// <summary>
        ///     The property to be updated
        /// </summary>
        public string Property { get; }

        public override int GetHashCode()
        {
            return (Target, Property).GetHashCode();
        }

        protected TPropertyType GetValue()
        {
            return _getter(Target);
        }

        protected void SetValue(TPropertyType value)
        {
            _setter(Target, value);
        }

        protected TPropertyType GetEndValue()
        {
            return _endValue.Invoke();
        }
    }
}