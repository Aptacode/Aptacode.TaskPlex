using System;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks.Transformations
{
    public sealed class StringTransformation<TClass> : PropertyTransformation<TClass, string> where TClass : class
    {
        private int _tickCount;

        /// <summary>
        ///     Update a string property on the target to the value returned by the given Func after the task duration
        /// </summary>
        private StringTransformation(TClass target,
            string property,
            Func<string> endValue,
            TimeSpan taskDuration) : base(target,
            property,
            endValue,
            taskDuration)
        {
        }

        public static StringTransformation<T> Create<T>(T target, string property, string endValue, TimeSpan duration)
            where T : class
        {
            return StringTransformation<T>.Create(target, property, () => endValue, duration);
        }

        public static StringTransformation<T> Create<T>(T target, string property, Func<string> endValue,
            TimeSpan duration) where T : class
        {
            try
            {
                return new StringTransformation<T>(target, property, endValue, duration);
            }
            catch
            {
                return null;
            }
        }

        protected override async Task InternalTask()
        {
            if (Duration.TotalMilliseconds < 10)
            {
                SetValue(GetEndValue());
                return;
            }

            _tickCount = 0;

            while (_tickCount < _stepCount && !CancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(1).ConfigureAwait(false);
            }

            SetValue(GetEndValue());
        }

        public override void Update()
        {
            if (!IsRunning())
            {
                return;
            }

            _tickCount++;
        }
    }
}