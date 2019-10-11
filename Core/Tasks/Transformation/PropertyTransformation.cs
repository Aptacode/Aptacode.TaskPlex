using System;
using System.Reflection;

namespace Aptacode.Core.Tasks.Transformations
{
    public abstract class PropertyTransformation : BaseTask
    {

        public object Target { get; set; }
        public PropertyInfo Property { get; set; }
        public TimeSpan TaskDuration { get; set; }
        public TimeSpan StepDuration { get; set; }
        public PropertyTransformation(object target, string property, TimeSpan taskDuration, TimeSpan stepDuration)
        {
            Target = target;
            Property = target.GetType().GetProperty(property);
            TaskDuration = taskDuration;
            StepDuration = stepDuration;
        }

        public override bool CollidesWith(BaseTask otherTask)
        {
            PropertyTransformation otherTransformation = otherTask as PropertyTransformation;
            if (otherTransformation != null)
                return Target == otherTransformation.Target && Property.Name == otherTransformation.Property.Name;
            else
                return false;
        }
    }

    public abstract class PropertyTransformation<T> : PropertyTransformation
    {
        public Func<T> DestinationValue { get; set; }
        public PropertyTransformation(object target, string property, Func<T> destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, taskDuration, stepDuration)
        {
            DestinationValue = destinationValue;
        }

        public PropertyTransformation(object target, string property, T destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, taskDuration, stepDuration)
        {
            DestinationValue = new Func<T>(() => { return destinationValue; });
        }

        protected T GetStartValue()
        {
            return (T)Property.GetValue(Target);
        }
        protected T GetEndValue()
        {
            return (T)DestinationValue.Invoke();
        }

        protected void SetValue(T value)
        {
            Property.SetValue(Target, value);
        }
    }
}
