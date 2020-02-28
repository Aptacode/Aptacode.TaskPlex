using System;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Tasks.Transformations
{
    public class UpdateAtEndTransformation<TClass, TPropertyType> : PropertyTransformation<TClass, TPropertyType> where TClass : class
    {
        private int _tickCount;

        internal UpdateAtEndTransformation(TClass target,
            string property,
            Func<TPropertyType> endValue,
            int stepCount) : base(target,
            property,
            endValue,
            stepCount)
        {
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