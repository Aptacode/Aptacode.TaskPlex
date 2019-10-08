using System;
using System.Reflection;

namespace TaskCoordinator.Tasks.Transformation.Interpolaton
{
    public class DoubleInterpolation : Interpolation<double>
    {
        public DoubleInterpolation(object target, PropertyInfo property, Func<double> destinationValue, TimeSpan duration) : base(target, property, destinationValue, duration)
        {

        }

        protected override double Add(double a, double b)
        {
            return a + b;
        }

        protected override double Divide(double a, int b)
        {
            return a / b;
        }

        protected override double Subtract(double a, double b)
        {
            return a - b;
        }
    }
}
