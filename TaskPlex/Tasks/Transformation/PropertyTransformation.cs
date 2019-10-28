using System;
using System.Reflection;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public abstract class PropertyTransformation : BaseTask
    {
        protected PropertyTransformation(object target, string property, TimeSpan duration, TimeSpan stepDuration) :
            base(duration)
        {
            Target = target;
            Property = target.GetType().GetProperty(property);
            StepDuration = stepDuration;
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
    }

    public abstract class PropertyTransformation<T> : PropertyTransformation
    {
        private readonly Action<T> setter;
        protected PropertyTransformation(object target, string property, Func<T> destinationValue, TimeSpan duration,
            TimeSpan stepDuration) : base(target, property, duration, stepDuration)
        {
            DestinationValue = destinationValue;
            this.setter = null;
        }

        protected PropertyTransformation(object target, string property, Func<T> destinationValue, Action<T> setter, TimeSpan duration,
            TimeSpan stepDuration) : base(target, property, duration, stepDuration)
        {
            DestinationValue = destinationValue;
            this.setter = setter;
        }

        protected PropertyTransformation(object target, string property, T destinationValue, TimeSpan duration,
            TimeSpan stepDuration) : base(target, property, duration, stepDuration)
        {
            DestinationValue = () => destinationValue;
            this.setter = null;
        }

        /// <summary>
        ///     When invoked returns the destination value of the transformation
        /// </summary>
        public Func<T> DestinationValue { get; set; }

        protected T GetStartValue()
        {
            return (T) Property.GetValue(Target);
        }

        protected T GetEndValue()
        {
            return DestinationValue.Invoke();
        }

        protected void SetValue(T value)
        {
            if (setter == null)
            {
                Property.SetValue(Target, value);
            }
            else
            {
                setter.Invoke(value);
            }
        }
    }
}