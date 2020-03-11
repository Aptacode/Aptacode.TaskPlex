using System;

namespace Aptacode.TaskPlex.Stories.Transformations
{
    public sealed class BoolTransformation<TClass> : UpdateAtEndTransformation<TClass, bool> where TClass : class
    {
        /// <summary>
        ///     Update a bool property on the target to the value returned by the given Func after the task duration
        /// </summary>
        public BoolTransformation(TClass target,
            string property,
            TimeSpan duration,
            bool endValue) : base(target,
            property,
            duration,
            endValue)
        {
        }
    }
}