using System;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public sealed class StringTransformation<TClass> : PropertyTransformation<TClass, string> where TClass : class
    {
        /// <summary>
        ///     Update a string property on the target to the value returned by the given Func after the task duration
        /// </summary>
        private StringTransformation(TClass target,
            string property,
            Func<string> endValue,
            TimeSpan taskDuration,
            RefreshRate refreshRate) : base(target,
            property,
            endValue,
            taskDuration,
            refreshRate)
        {
        }

        public static StringTransformation<T> Create<T>(T target, string property, string endValue, TimeSpan duration,
            RefreshRate refreshRate) where T : class
        {
            return StringTransformation<T>.Create(target, property, () => endValue, duration, refreshRate);
        }

        public static StringTransformation<T> Create<T>(T target, string property, Func<string> endValue,
            TimeSpan duration, RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            try
            {
                return new StringTransformation<T>(target, property, endValue, duration, refreshRate);
            }
            catch
            {
                return null;
            }
        }

        protected override async Task InternalTask()
        {
            await Task.Delay(Duration, CancellationToken.Token).ConfigureAwait(false);
            await WaitUntilResumed().ConfigureAwait(false);
            SetValue(GetEndValue());
        }
    }
}