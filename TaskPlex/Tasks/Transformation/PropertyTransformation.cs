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
        public object Target { get; set; }

        /// <summary>
        ///     The property to be updated
        /// </summary>
        public PropertyInfo Property { get; set; }

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
        protected PropertyTransformation(object target, string property, Func<T> destinationValue, TimeSpan duration,
            TimeSpan stepDuration) : base(target, property, duration, stepDuration)
        {
            DestinationValue = destinationValue;
        }

        protected PropertyTransformation(object target, string property, T destinationValue, TimeSpan duration,
            TimeSpan stepDuration) : base(target, property, duration, stepDuration)
        {
            DestinationValue = () => destinationValue;
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
            Property.SetValue(Target, value);
        }
    }
}