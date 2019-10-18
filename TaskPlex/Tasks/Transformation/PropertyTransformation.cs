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

        public object Target { get; set; }
        public PropertyInfo Property { get; set; }
        public TimeSpan StepDuration { get; set; }

        public override bool CollidesWith(BaseTask otherTask)
        {
            if (otherTask is PropertyTransformation otherTransformation)
            {
                return Target == otherTransformation.Target && Property.Name == otherTransformation.Property.Name;
            }

            return false;
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