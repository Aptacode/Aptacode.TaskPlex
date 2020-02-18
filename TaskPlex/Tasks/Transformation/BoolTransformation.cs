using System;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public sealed class BoolTransformation<TClass> : PropertyTransformation<TClass, bool> where TClass : class
    {
        /// <summary>
        ///     Update a string property on the target to the value returned by the given Func after the task duration
        /// </summary>
        private BoolTransformation(TClass target,
            string property,
            Func<bool> endValue,
            TimeSpan taskDuration,
            RefreshRate refreshRate = RefreshRate.Normal) : base(target,
            property,
            endValue,
            taskDuration,
            refreshRate)
        {
        }
        public static BoolTransformation<T> Create<T>(T target, string property, bool endValue, TimeSpan duration, RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            try
            {
                return new BoolTransformation<T>(target, property, () => endValue, duration, refreshRate);
            }
            catch
            {
                return null;
            }
        }

        protected override async Task InternalTask()
        {
            await Task.Delay(Duration, CancellationToken.Token).ConfigureAwait(false);
            SetValue(GetEndValue());
        }
    }
}