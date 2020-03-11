using System;

namespace Aptacode.TaskPlex.Stories.Transformations
{
    public sealed class StringTransformation<TClass> : UpdateAtEndTransformation<TClass, string> where TClass : class
    {
        /// <summary>
        ///     Update a string property on the target to the value returned by the given Func after the task duration
        /// </summary>
        public StringTransformation(TClass target,
            string property,
            TimeSpan duration,
            string endValue) : base(target,
            property,
            duration,
            endValue)
        {
        }
    }
}