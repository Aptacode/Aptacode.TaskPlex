using System;
using System.Reflection;

namespace Aptacode_PropertyTransposer.Transformation
{
    public abstract class Transformation
    {
        public event EventHandler<PropertyActionEventArgs> OnStarted;
        public event EventHandler<PropertyActionEventArgs> OnFinished;
        public TimeSpan Interval = TimeSpan.FromMilliseconds(20);

        public object Target { get; set; }
        public PropertyInfo Property { get; set; }
        public TimeSpan Duration { get; set; }
        public Transformation(object target, PropertyInfo property, TimeSpan duration)
        {
            Target = target;
            Property = property;
            Duration = duration;
        }

        public abstract void Start();

        protected void Started()
        {
            OnStarted?.Invoke(this, new PropertyActionEventArgs());
        }

        protected void Finished()
        {
            OnFinished?.Invoke(this, new PropertyActionEventArgs());
        }
    }

    public abstract class Transformation<T> : Transformation
    {
        public Func<T> DestinationValue { get; set; }
        public Transformation(object target, PropertyInfo property, Func<T> destinationValue, TimeSpan duration) : base(target, property, duration)
        {
            DestinationValue = destinationValue;
        }

        protected T GetStartValue()
        {
            return (T)Property.GetValue(Target);
        }
        protected T GetEndValue()
        {
            return (T)DestinationValue.Invoke();
        }

        protected void UpdateValue(T value)
        {
            Property.SetValue(Target, value);
        }
    }
}
