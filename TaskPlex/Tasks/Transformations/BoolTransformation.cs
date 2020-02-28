using System;

namespace Aptacode.TaskPlex.Tasks.Transformations
{
    public sealed class BoolTransformation<TClass> : UpdateAtEndTransformation<TClass, bool> where TClass : class
    {
        /// <summary>
        ///     Update a bool property on the target to the value returned by the given Func after the task duration
        /// </summary>
        internal BoolTransformation(TClass target,
            string property,
            Func<bool> endValue,
            int stepCount) : base(target,
            property,
            endValue,
            stepCount)
        {
        }

        public static BoolTransformation<T> Create<T>(T target, string property, bool endValue, int duration)
            where T : class
        {
            try
            {
                return new BoolTransformation<T>(target, property, () => endValue, duration);
            }
            catch
            {
                return null;
            }
        }
    }
}