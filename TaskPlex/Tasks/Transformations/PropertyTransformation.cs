using System;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Tasks.Transformations
{
    public abstract class PropertyTransformation : BaseTask
    {
        protected PropertyTransformation(TimeSpan duration) : base(duration)
        {
        }
    }

    public abstract class PropertyTransformation<TClass, TPropertyType> : PropertyTransformation where TClass : class
    {
        private readonly Func<TPropertyType> _endValue;
        private readonly Func<TClass, TPropertyType> _getter;
        private readonly Action<TClass, TPropertyType> _setter;
        protected readonly int StepCount;

        protected PropertyTransformation(TClass target,
            string property,
            Func<TPropertyType> endValue,
            TimeSpan duration,
            RefreshRate refreshRate) : base(duration)
        {
            Target = target;
            Property = property;
            var propertyInfo = typeof(TClass).GetProperty(Property);

            _setter = (Action<TClass, TPropertyType>) Delegate.CreateDelegate(typeof(Action<TClass, TPropertyType>),
                null, propertyInfo.GetSetMethod());
            _getter = (Func<TClass, TPropertyType>) Delegate.CreateDelegate(typeof(Func<TClass, TPropertyType>),
                null,
                propertyInfo.GetGetMethod());
            _endValue = endValue;
            RefreshRate = refreshRate;
            StepCount = (int) Math.Floor(Duration.TotalMilliseconds / (int) RefreshRate);
        }

        public TClass Target { get; }
        public string Property { get; }
        protected RefreshRate RefreshRate { get; }

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