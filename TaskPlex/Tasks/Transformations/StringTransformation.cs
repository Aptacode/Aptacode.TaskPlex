using System;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Tasks.Transformations
{
    public sealed class StringTransformation<TClass> : PropertyTransformation<TClass, string> where TClass : class
    {
        private int _tickCount;

        /// <summary>
        ///     Update a string property on the target to the value returned by the given Func after the task duration
        /// </summary>
        internal StringTransformation(TClass target,
            string property,
            Func<string> endValue,
            int stepCount) : base(target,
            property,
            endValue,
            stepCount)
        {
        }

        public static StringTransformation<T> Create<T>(T target, string property, string endValue, int stepCount)
            where T : class
        {
            return StringTransformation<T>.Create(target, property, () => endValue, stepCount);
        }

        public static StringTransformation<T> Create<T>(T target, string property, Func<string> endValue,
            int stepCount) where T : class
        {
            try
            {
                return new StringTransformation<T>(target, property, endValue, stepCount);
            }
            catch
            {
                return null;
            }
        }

        protected override void Setup()
        {
            _tickCount = 0;
        }

        protected override void Begin()
        {
        }

        protected override void Cleanup()
        {
            _tickCount = 0;
        }

        public override void Update()
        {
            if (CancellationTokenSource.IsCancellationRequested)
            {
                Finished();
            }

            if (!IsRunning())
            {
                return;
            }

            if (++_tickCount >= StepCount)
            {
                SetValue(GetEndValue());
                Finished();
            }
        }

        public override void Reset()
        {
            State = TaskState.Paused;
            Cleanup();
        }
    }
}