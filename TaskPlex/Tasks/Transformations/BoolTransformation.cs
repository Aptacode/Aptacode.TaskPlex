using System;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Tasks.Transformations
{
    public sealed class BoolTransformation<TClass> : PropertyTransformation<TClass, bool> where TClass : class
    {
        private int _tickCount;

        /// <summary>
        ///     Update a string property on the target to the value returned by the given Func after the task duration
        /// </summary>
        private BoolTransformation(TClass target,
            string property,
            Func<bool> endValue,
            TimeSpan taskDuration) : base(target,
            property,
            endValue,
            taskDuration)
        {
        }

        public static BoolTransformation<T> Create<T>(T target, string property, bool endValue, TimeSpan duration)
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

        protected override async Task InternalTask(RefreshRate refreshRate)
        {
            if (Duration.TotalMilliseconds < 10)
            {
                SetValue(GetEndValue());
                return;
            }

            State = TaskState.Running;
            _tickCount = 0;

            while (State != TaskState.Stopped && !CancellationTokenSource.IsCancellationRequested)
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

            if (++_tickCount < _stepCount)
            {
                return;
            }

            State = TaskState.Stopped;
        }
    }
}