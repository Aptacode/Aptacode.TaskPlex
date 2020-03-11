using System;
using System.Threading;

namespace Aptacode.TaskPlex.Stories.Transformations
{
    public abstract class PropertyTransformation : BaseStory
    {
        protected PropertyTransformation(TimeSpan duration) : base(duration)
        {
        }
    }

    public abstract class PropertyTransformation<TClass, TPropertyType> : PropertyTransformation where TClass : class
    {
        private readonly Func<TClass, TPropertyType> _getter;
        private readonly Action<TClass, TPropertyType> _setter;
        protected readonly bool UseStartValue;
        protected readonly TPropertyType[] Values;

        protected PropertyTransformation(
            TClass target,
            string property,
            TimeSpan duration,
            bool useStartValue = true,
            params TPropertyType[] values) : base(duration)
        {
            Target = target;
            Property = property;
            var propertyInfo = typeof(TClass).GetProperty(Property);

            _setter = (Action<TClass, TPropertyType>) Delegate.CreateDelegate(typeof(Action<TClass, TPropertyType>),
                null, propertyInfo?.GetSetMethod() ?? throw new InvalidOperationException());
            _getter = (Func<TClass, TPropertyType>) Delegate.CreateDelegate(typeof(Func<TClass, TPropertyType>),
                null,
                propertyInfo.GetGetMethod());

            Values = values;
            UseStartValue = useStartValue;
        }

        public TClass Target { get; }
        public string Property { get; }

        protected TPropertyType GetValue() => _getter(Target);

        protected void SetValue(TPropertyType value) => _setter(Target, value);
    }
}