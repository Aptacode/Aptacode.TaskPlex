using System;
using System.Threading;

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

        public SynchronizationContext SynchronizationContext { get; set; }

        public TClass Target { get; }
        public string Property { get; }

        protected TPropertyType GetValue()
        {
            if (SynchronizationContext == null)
            {
                return _getter(Target);
            }

            var result = new SynchronizedResult();

            SynchronizationContext.Send(o => ((SynchronizedResult) o).Result = _getter(Target), result);

            return result.Result;
        }

        protected void SetValue(TPropertyType value)
        {
            if (SynchronizationContext == null)
            {
                _setter(Target, value);
            }
            else
            {
                SynchronizationContext.Post(o => _setter(Target, value), value);
            }
        }

        private class SynchronizedResult
        {
            internal TPropertyType Result { get; set; }
        }
    }
}