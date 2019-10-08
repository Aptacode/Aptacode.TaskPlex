using System;
using System.Reflection;

namespace Aptacode_PropertyTransposer.Transformation.Interpolaton
{
    public abstract class Interpolation<T> : Transformation<T>
    {
        public Interpolation(object target, PropertyInfo property, Func<T> destinationValue, TimeSpan duration) : base(target, property, destinationValue, duration)
        {

        }
    }
}
