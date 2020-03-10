using System;
using Aptacode.TaskPlex.Engine.Enums;

namespace Aptacode.TaskPlex.Engine.Tasks.Transformations
{
    public class UpdateAtEndTransformation<TClass, TPropertyType> : PropertyTransformation<TClass, TPropertyType>
        where TClass : class
    {
        private int _tickCount;

        internal UpdateAtEndTransformation(TClass target,
            string property,
            TimeSpan duration,
            TPropertyType value) : base(target,
            property,
            duration,
            false,
            value)
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
            if (IsCancelled)
            {
                Finished();
            }

            if (!IsRunning())
            {
                return;
            }

            if (++_tickCount >= StepCount)
            {
                SetValue(Values[0]);
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