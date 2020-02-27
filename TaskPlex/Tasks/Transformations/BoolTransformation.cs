using System;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Tasks.Transformations
{
    public sealed class BoolTransformation<TClass> : PropertyTransformation<TClass, bool> where TClass : class
    {
        private int _tickCount;

        /// <summary>
        ///     Update a string property on the target to the value returned by the given Func after the task duration
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
            _tickCount = 0;
        }
    }
}